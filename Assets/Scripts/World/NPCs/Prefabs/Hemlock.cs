public class Hemlock : NPC
{
    private void Start()
    {
        name = "Hemlock";
    }
    public override void Interaction()
    {
        if (CanInteract) 
        {
            if (!HasFlag("Met")) 
            { 
                CanInteract = false;
                TextBox.text = "Hey. Name's Hemlock.";
                WaitForInput();
                TextBox.text = "Come and warm yourself by the fire.\nIt will set you a respawn and save your game.";
                WaitForInput();
                CanInteract = true;
                return;
            } 
            if (HasFlag("SwordAhead"))
            {
                CanInteract = false;
                TextBox.text = "Can you feel it?";
                WaitForInput();
                TextBox.text = "There is a powerful weapon ahead!";
                WaitForInput();
                TextBox.text = "Take it!";
                WaitForInput();
                CanInteract = true;
                return;
            }
            if (HasFlag("SwordTaken")) 
            { 
                CanInteract = false;
                TextBox.text = "Now you can defend yourself!";
                WaitForInput();
                TextBox.text = "Try attacking that dummy over there.";
                WaitForInput();
                CanInteract = true;
                return;
            }
        }
    }
}

