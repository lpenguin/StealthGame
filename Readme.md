# Stealth Game Prototype

This short demo is intented to be my Unity portfolio project. It's a small game in a stealth genre. It covers most of the Unity systems such as navigation, lighting, animation, audio and mostly focused on AI. 

![Screenshot](_images/Screenshot.png?raw=true)

## AI Behaviour Trees ([SimpleBT](./Assets/Scripts/SimpleBT))

I choosed to build my own BT system because none of the free BT Unity assets are good and for skills demostraion purposes. It was designed with a KISS principle in mind, although no heavy performance optimisation was done, it's highly extendable and contains most of the basic building bricks.

### Basic features
* A single Node task class instead of complicated division of Action, Condition, etc classes.
* Blackboard support
![BT Component](_images/BehaviourTreeComponent.png?raw=true)
* YAML syntax. There's no GUI editor and BT definition is in YAML files
```yaml
  Wait:
    Wait: 1

  Wait3:
    Wait: 3

  LookAround:
    Sequence:
    - Robot.EnableLookAt: true
    - RandomSelector:        
      - Animator.PlayState: { state: "Look Around Fast" , layer: "Actions" }
    - Robot.EnableLookAt: false
  
      
  Transition_ToAlerted:
    Sequence:
    - Selector.Active.HasReceivedEvent: { "Check Position", $"Check Position Source", $"Check Position Sense", $"Check Position", comment: "Check Position" }
    - Selector:
      - Str.Equals: { $State, "Alerted" }      
      - Audio.Play: "I hear something"
    - Str.Set: { $State, "Alerted" }
```
* Basic Composite Nodes: Selector, Sequence, Parallel, Race
* Interrupts support: Selector.Active node which constantly re-evaluates child nodes with low priority 
* BT Window displays current state of the tree 
![BT Window](_images/BehaviourTreeWindow.png?raw=true)
* Subtree support


