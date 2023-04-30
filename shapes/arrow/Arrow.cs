namespace Flowcharter.shapes.arrow
{
    [Tool]
    public partial class Arrow : Sprite2D
    {
        public float tailLengthModifier = 1;
        [Export] private float TailLengthModifier
        {
            get => tailLengthModifier; 
            set
            {
                tailLengthModifier = value;
                TailLengthModified(value);
                GD.Print("Modified");
            }
        }
        Rect2 tailRect = new Rect2();
        private void TailLengthModified(float value)
        {
            GetNode<Sprite2D>("Sprite2D").RegionRect = new Rect2(Vector2.Zero, 48, 48 * value);
        }
        private void alinus_abuke()
        {
            
        }
    }
}