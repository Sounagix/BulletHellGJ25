using System;

public enum SceneID
{
    LoadingScene = 0,
    Menu = 1,
    Game = 2,
}

public enum GameState
{
    MainMenu,
    Playing,
    Pause,
    GameOver
}

[Serializable]
public enum InteractableType 
{
    Food,
    Weapon
}

[Serializable]
public enum WeaponType 
{
    Knife, 
    Bomb,
    Fire,
    Fish,
    dumbbell,
    Sneakers,
}

[Serializable]
public enum FoodType 
{
    Burger,
    Fries,
    Cake,
    Hotdog,
}

[Serializable]
public enum CustomerState
{
    Spawned,
    Normal,
    Unstable,
    Served,
}