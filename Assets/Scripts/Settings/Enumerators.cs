namespace Settings
{
    public class Enumerators
    {
        public enum AppState
        {
            Unknown,

            Main,
            Game,
            Lose,
            Win,
        }
        
        
        public enum InputType
        {
            Unknown,

            Mouse,
            Keyboard,
            Swipe,
            Joystick
        }
        
        public enum Direction
        {
            Unknown,

            Left,
            Right,
            Up,
            Down
        }
    }
}