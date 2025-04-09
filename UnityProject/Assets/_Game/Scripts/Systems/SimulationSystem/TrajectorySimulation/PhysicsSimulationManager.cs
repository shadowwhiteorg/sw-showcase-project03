using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Systems.SimulationSystem
{
    public class PhysicsSimulationManager : MonoBehaviour
{
    [Header("Simulation Settings")]
    [Tooltip("Simulation ball prefab (must have BallTrajectoryRecorder attached)")]
    public GameObject simulationBallPrefab;
    [Tooltip("Duration (in seconds) to simulate at most")]
    public float simulationDuration = 5f;
    [Tooltip("Velocity threshold below which simulation is considered finished")]
    public float stopVelocityThreshold = 0.1f;
    [Tooltip("Initial force/velocity to apply to the ball in simulation")]
    public Vector3 initialForce = new Vector3(0, 0, 10f);

    [Header("References")]
    [Tooltip("Reference to the wheel center (for outcome detection)")]
    public Transform wheelCenter;
    [Tooltip("Total number of slots on the wheel")]
    public int totalSlots = 37;

    // Event for notifying simulation complete
    public delegate void SimulationCompleteHandler(TrajectoryData data);
    public event SimulationCompleteHandler SimulationComplete;

    // The separate physics scene used for simulation.
    private Scene simulationScene;
    private PhysicsScene physicsScene;

    // The simulation ball instance and its components.
    private GameObject simBallInstance;
    private BallTrajectoryRecorder recorder;
    private Rigidbody simBallRb;

    // Call this method to start simulation. The parameters passed (such as ball start position)
    // can be obtained from your current ball’s settings.
    public void StartSimulationSpin(Vector3 ballStartPosition, Quaternion ballStartRotation)
    {
        // Create a new simulation scene if it doesn’t exist.
        if (!simulationScene.IsValid())
        {
            CreateSimulationScene();
        }

        // Instantiate the simulation ball into the simulation scene.
        if (simBallInstance == null)
        {
            simBallInstance = Instantiate(simulationBallPrefab, ballStartPosition, ballStartRotation);
            // Move the simulation ball into our dedicated physics scene.
            SceneManager.MoveGameObjectToScene(simBallInstance, simulationScene);
            recorder = simBallInstance.GetComponent<BallTrajectoryRecorder>();
            simBallRb = simBallInstance.GetComponent<Rigidbody>();
        }
        else
        {
            // Reset recorder and reinitialize ball position.
            recorder.trajectoryData.samples.Clear();
            simBallInstance.transform.position = ballStartPosition;
            simBallInstance.transform.rotation = ballStartRotation;
        }

        // Ensure the simulation ball is active.
        simBallInstance.SetActive(true);

        // Prepare the ball for physics simulation.
        simBallRb.isKinematic = false;
        simBallRb.linearVelocity = Vector3.zero;
        simBallRb.angularVelocity = Vector3.zero;

        // Apply initial force.
        simBallRb.AddForce(initialForce, ForceMode.VelocityChange);

        // Begin the simulation coroutine.
        StartCoroutine(SimulationRoutine());
    }

    // Create a new physics scene for simulation.
    void CreateSimulationScene()
    {
        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        simulationScene = SceneManager.CreateScene("SimulationScene", csp);
        physicsScene = simulationScene.GetPhysicsScene();
    }

    IEnumerator SimulationRoutine()
    {
        float elapsedSim = 0f;
        float fixedDeltaTime = Time.fixedDeltaTime;

        // Manually simulate physics steps.
        while (elapsedSim < simulationDuration)
        {
            // Step the simulation manually.
            physicsScene.Simulate(fixedDeltaTime);
            elapsedSim += fixedDeltaTime;

            // End simulation if the ball’s velocity is below threshold.
            if (simBallRb.linearVelocity.magnitude < stopVelocityThreshold)
            {
                break;
            }

            yield return null; // Wait one frame before simulating next step.
        }
        
        // Optional: simulate a few extra steps for stabilization.
        yield return new WaitForSeconds(0.1f);

        // Stop the simulation ball.
        simBallRb.isKinematic = true;

        // Retrieve the recorded trajectory.
        TrajectoryData data = recorder.GetTrajectoryData();
        // Determine winning number using final recorded position.
        data.winningNumber = DetermineWinningNumber(data);

        // Deactivate simulation ball.
        simBallInstance.SetActive(false);

        // Fire simulation complete event.
        SimulationComplete?.Invoke(data);
    }

    // Example outcome determination based on ball position relative to wheel center.
    int DetermineWinningNumber(TrajectoryData data)
    {
        if (data.samples.Count == 0)
            return -1; // Error case

        TrajectorySample lastSample = data.samples[data.samples.Count - 1];
        Vector3 dir = lastSample.position - wheelCenter.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;
        float slotAngle = 360f / totalSlots;
        int slotIndex = Mathf.FloorToInt(angle / slotAngle);
        // You may map slotIndex to the actual roulette number here.
        return slotIndex;
    }
}
}