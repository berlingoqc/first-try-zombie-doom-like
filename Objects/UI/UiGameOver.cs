using Godot;
using System;

public class UiGameOver : Control
{
    public override void _Ready()
    {

        var btnQuit = GetNode<Button>("Panel/BtnQuitGame");
        var btnPlay = GetNode<Button>("Panel/BtnPlayAgain");

        btnPlay.Connect("pressed", this, "_on_BtnNewGame");
        btnQuit.Connect("pressed", this, "_on_BtnQuitGame");

        Hide();
    }

    public void ShowDialog(int kills, int rounds)
    {
        var lblKills = GetNode<Label>("Panel/GridContainer/LblScoreKill");
        var lblRounds = GetNode<Label>("Panel/GridContainer/LblScoreRound");
        lblKills.Text = "" + kills;
        lblRounds.Text = "" + (rounds + 1);
        this.Show();
    }

    public void _on_BtnNewGame()
    {
        GetTree().ReloadCurrentScene();
    }

    public void _on_BtnQuitGame()
    {
        GetTree().Quit();
    }

}
