using System;

public enum SceneID
{
    LoadingScene = 0,
    Menu = 1,
    Game = 2,
    GameOver = 3,
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
    None,
    Knife, 
    Bomb,
    Fire,
    Fish,
    dumbbell,
    Sneakers,
    Hand,
    Eye,
    Ball,
    Proteine,
    Pen,

}

[Serializable]
public enum FoodType 
{
    None,
    Burger,
    Fries,
    Cake,
    Hotdog,
    Croissant,
    Pizza,
    FishAndChips,
    Taco,
    Paella,
}

[Serializable]
public enum CustomerState
{
    Spawned,
    Normal,
    Unstable,
    Served,
}