using SimpleBT.Attributes;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes.AudioManager
{
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
}