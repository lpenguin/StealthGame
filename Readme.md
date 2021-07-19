# Stealth Game Prototype
[Download](https://github.com/lpenguin/StealthGame/releases/tag/v0.0.2)  
This short demo is my Unity portfolio project. It's a small game in the stealth genre. It covers most of the Unity systems such as navigation, lighting, animation, audio, character control and is mostly focused on AI.

![Screenshot](_images/Screenshot.PNG?raw=true)

## AI Behaviour Trees ([SimpleBT](./Assets/Scripts/SimpleBT))

I chose to build my own BT system because none of the free BT Unity assets are good and for skills demonstration purposes. It was designed with a KISS principle in mind, although no heavy performance optimisation was done, it's highly extendable and contains most of the basic building bricks.

### Basic features
* A single Node task class instead of complicated division of Action, Condition, etc classes.
* Basic Composite Nodes: Selector, Sequence, Parallel, Race
* Interrupts support: Selector.Active node which constantly re-evaluates child nodes with low priority
* Subtrees support
* YAML syntax. There's no GUI editor and BTs are defined in YAML syntax
```yaml

  LookAround:
    Sequence:
    - Robot.EnableLookAt: true
    - RandomSelector:        
      - Animator.PlayState: { state: "Look Around Fast" , layer: "Actions" }
    - Robot.EnableLookAt: false
 
  Transition_ToAggressive:
    Sequence:
    - Vision.HasTarget: { sensor: $Vision Sensor, target: $Player }
    - Sequence:
      - Success:
        - Sequence:
          - Not:
            - Str.In: { $State, ["Searching", "Aggressive"] }
          - Audio.Play: "Intruder"
      - Animator.SetTrigger: { "Interrupt Action" }
      - Str.Set: { $State, "Aggressive" }
```
* Node parameterization. One can define a custom Node class with parameters covering all basic types: string, int, float, Vector3, Transform
```C#
[Name("Audio.Play")]
public class AudioPlay: Node
{
    private StringParameter name;
    private Audio.AudioManager audioManager;

    protected override void OnStart()
    {
        audioManager = currentContext.GameObject.GetComponent<Audio.AudioManager>();
    }

    protected override Status OnUpdate()
    {
        audioManager.PlayAudio(name);
        return Status.Success;
    }
}
```
* Blackboard support. One can bind Blackboard values to the BT Node parameter
``` YAML
Vision.HasTarget: { sensor: $Vision Sensor, target: $Player }
```
![BT Component](_images/BehaviorTreeComponent.PNG?raw=true)
* BT Window for visualisation of the current state of the tree
![BT Window](_images/BehaviorTreeWindow.PNG?raw=true)

### Enemy Robot AI implementation
* Behaviour trees as a core (decision making, plan execution)
* FSM inside of Behaviour tree for better handling Robot states and transitions between them: Idle, Alerted, Aggressive, etc. FSM is emulated via Selector.Active node. This kind of architecture was chosen because I was impressed by the [Bobby Anguelov GDC AI talk](https://www.youtube.com/watch?v=Qq_xX1JCreI). In short, BTs are not good with the state management and it's better to combine FSM (state management) and Trees (actual plan execution). I decided to not implement a separate FSM component and emulate it with BT.

## Other notable features
### 2D convex hull collider
It's used in complex vision sensor shape. One can edit it with the gui similar to the polygon collider editor.
![BT Window](_images/ConvexHull2d.PNG?raw=true)

### AI Patrol Route
This component describes a cycled route for an AI agent. One can assign a tree execution (Wait, Look Around, etc) for a particular position of the route.
![BT Window](_images/PatrolRoute.PNG?raw=true)

