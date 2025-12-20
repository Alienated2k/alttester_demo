using AltTester.AltTesterSDK.Driver;
using NUnit.Framework;

public class HalloweenNightmareSmokeTest
{
    #region Setup
    public AltDriver driver;

    [OneTimeSetUp]
    public void SetUp()
    {
        driver = new AltDriver(
            host: "127.0.0.1",
            port: 13000,
            appName: "halloween_nightmare",
            enableLogging: false,
            connectTimeout: 60,
            platform: "WindowsEditor",
            platformVersion: "unknown",
            deviceInstanceId: "unknown",
            appId: "unknown",
            driverType: "SDK");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Stop();
    }
    #endregion

    #region String Constants

    private const string MainMenuSceneName = "Main Menu";
    private const string GraveyardSceneName = "Graveyard";
    private const string IntroText = "Turn over the graveyard and find all the sweets!";
    private const string BossFightText = "You've been so bad, the dead have risen from their graves!\n\nFight them back!";
    private const string VictoryText = "Your pockets are full of candy, the graveyard is destroyed," +
            "\nand evil has been defeated." +
            "\n\nYou had a great time!" +
            "\n\nSee you soon, and good luck!";
    private const string SecretText = "Dedicated to all my close and loyal friends...\n\n\n\n\n\n\n";

    #endregion

    #region Tests

    [Test, Order(0)]
    public void Test_GetCurrentScene()
    {
        var name = driver.GetCurrentScene();

        Assert.AreEqual(MainMenuSceneName, name);
    }

    [Test, Order(1)]
    public void Test_FindPressStartButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Start Game Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition);

        Assert.NotNull(button);
    }

    [Test, Order(2)]
    public void Test_GetLoadedScene()
    {
        var name = driver.GetCurrentScene();

        Assert.AreEqual(GraveyardSceneName, name);
    }

    [Test, Order(3)]
    public void FindInteractableObjectsAndDisableCollidersOnThem()
    {
        var bench = driver.FindObject(By.NAME, "Bench Decorated");
        bench.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", false, "UnityEngine.PhysicsModule");
        var scull = driver.FindObject(By.NAME, "Scull Candle");
        scull.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", false, "UnityEngine.PhysicsModule");
        var shrine = driver.FindObject(By.NAME, "Shrine");
        shrine.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", false, "UnityEngine.PhysicsModule");
        var plaque = driver.FindObject(By.NAME, "Plaque Candles");
        plaque.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", false, "UnityEngine.PhysicsModule");
        var shrineWithCandles = driver.FindObjects(By.NAME, "Shrine Candles");

        for (int i = 0; i < shrineWithCandles.Count; i++)
        {
            shrineWithCandles[i].SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", false, "UnityEngine.PhysicsModule");
        }
    }

    [Test, Order(4)]
    public void Test_WaitForFirstObjectiveWindowAndCheckText()
    {
        var window = driver.WaitForObject(By.NAME, "Mission Objective 1 Text");
        var windowText = window.GetText();

        Assert.AreEqual(IntroText, windowText);
    }

#if UNITY_STANDALONE_WIN
    [Test, Order(5)]
    public void Test_MoveCameraBackwardAndCheckPosition()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();

        driver.PressKey(AltKeyCode.S, 1, 5.0f);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }

    [Test, Order(6)]
    public void Test_MoveCameraForwardAndCheckPosition()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();

        driver.PressKey(AltKeyCode.W, 1, 2.5f);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }
#elif UNITY_ANDROID
    [Test, Order(5)]
    public void Test_FindCameraBackwardButtonAndClickOnThem()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();
        var button = driver.FindObject(By.NAME, "Backward Button");

        driver.HoldButton(button.GetScreenPosition(), 5.0f);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }

    [Test, Order(6)]
    public void Test_FindCameraForwardButtonAndClickOnThem()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();
        var button = driver.FindObject(By.NAME, "Forward Button");

        driver.HoldButton(button.GetScreenPosition(), 2.5f);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }
#endif

#if UNITY_STANDALONE_WIN
    [Test, Order(7)]
    public void Test_RotateCameraLeftAndCheckPosition()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();
        var power = 0.125f;
        var offset = 1.0f;
        var duration = 1.0f;

        driver.KeyDown(AltKeyCode.Mouse1, power);
        driver.MoveMouse(new AltVector2(offset, default), -duration);
        driver.KeyUp(AltKeyCode.Mouse1);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }

    [Test, Order(8)]
    public void Test_RotateCameraRightAndCheckPosition()
    {
        var camera = driver.FindObject(By.TAG, "MainCamera");
        var cameraStartPosition = camera.GetWorldPosition();
        var power = 0.125f;
        var offset = 1.0f;
        var duration = 1.0f;

        driver.KeyDown(AltKeyCode.Mouse1, power);
        driver.MoveMouse(new AltVector2(-offset, default), -duration);
        driver.KeyUp(AltKeyCode.Mouse1);
        camera.UpdateObject();

        var cameraEndPosition = camera.GetWorldPosition();

        Assert.AreNotEqual(cameraStartPosition, cameraEndPosition);
    }
#elif UNITY_ANDROID
	[Test, Order(7)]
    public void Test_FindJoystickAndRotateCameraLeft()
    {
        var joystick = driver.FindObject(By.NAME, "MobileJoystick");
        var joystickStartPosition = joystick.GetScreenPosition();
        var offsetX = 100;

        driver.BeginTouch(joystickStartPosition);
        driver.MoveTouch(1, new AltVector2(joystickStartPosition.x + offsetX, joystickStartPosition.y));

        joystick.UpdateObject();

        var joystickEndPosition = joystick.GetScreenPosition();
        driver.EndTouch(1);

        Assert.AreNotEqual(joystickStartPosition, joystickEndPosition);
    }

    [Test, Order(8)]
    public void Test_FindJoystickAndRotateCameraRight()
    {
        var joystick = driver.FindObject(By.NAME, "MobileJoystick");
        var joystickStartPosition = joystick.GetScreenPosition();
        var offsetX = 100;

        driver.BeginTouch(joystickStartPosition);
        driver.MoveTouch(1, new AltVector2(joystickStartPosition.x - offsetX, joystickStartPosition.y));

        joystick.UpdateObject();

        var joystickEndPosition = joystick.GetScreenPosition();
        driver.EndTouch(1);

        Assert.AreNotEqual(joystickStartPosition, joystickEndPosition);
    }
#endif

    [Test, Order(9)]
    public void Test_FindCandiesAndClickOnThem()
    {
        var component = driver.FindObject(By.NAME, "Canvas");
        var currentScore = component.GetComponentProperty<int>("ScoreManager",
            "_minScore", "Assembly-CSharp");
        var maxScore = component.GetComponentProperty<int>("ScoreManager",
            "_maxScore", "Assembly-CSharp");
        var candiesTotalCount = maxScore;

        while (currentScore < maxScore)
        {
            var candy = driver.WaitForObject(By.TAG, "Collectible Object");
            var candyPosition = candy.GetScreenPosition();

            driver.Tap(candyPosition, 1);

            currentScore++;
        }

        if (currentScore.Equals(maxScore))
        {
            for (int i = 0; i.Equals(candiesTotalCount); i++)
            {
                var candy = driver.FindObjects(By.TAG, "Collectible Object");

                Assert.IsFalse(candy[i].enabled);
            }
        }
    }

    [Test, Order(10)]
    public void Test_FindCandiesCounterAndCheckCandiesCount()
    {
        var component = driver.FindObject(By.NAME, "Canvas");
        var candiesTotalCount = component.GetComponentProperty<int>("ScoreManager",
            "_maxScore", "Assembly-CSharp");
        var candiesCurrentCount = component.WaitForComponentProperty<int>("ScoreManager",
            "_score", candiesTotalCount, "Assembly-CSharp");

        Assert.AreEqual(candiesTotalCount, candiesCurrentCount);
    }

    [Test, Order(11)]
    public void Test_WaitForSecondObjectiveWindowAndCheckText()
    {
        var window = driver.WaitForObject(By.NAME, "Mission Objective 2 Text");
        var windowText = window.GetText();

        Assert.AreEqual(BossFightText, windowText);
    }

    [Test, Order(12)]
    public void Test_FindMageAndClickOnThem()
    {
        var mage = driver.WaitForObject(By.TAG, "Mage Object");
        var mageMinHealth = mage.GetComponentProperty<int>("MageHealthController", "_minHealth", "Assembly-CSharp");
        var mageCurrentHealth = mage.GetComponentProperty<int>("MageHealthController", "_maxHealth", "Assembly-CSharp");

        while (mageCurrentHealth > mageMinHealth)
        {
            var magePosition = mage.GetScreenPosition();
            driver.Tap(magePosition, 1);

            mageCurrentHealth--;
        }

        if (mageCurrentHealth.Equals(mageMinHealth))
        {
            var activeSelf = mage.WaitForComponentProperty<bool>("UnityEngine.Transform",
                "gameObject.activeSelf", false, "UnityEngine.CoreModule");

            Assert.IsFalse(activeSelf);
        }
    }

    [Test, Order(13)]
    public void Test_WaitForThirdObjectiveWindowAndCheckText()
    {
        var window = driver.WaitForObject(By.NAME, "End Game Text");
        var windowText = window.GetText();

        Assert.AreEqual(VictoryText, windowText);
    }

    [Test, Order(14)]
    public void Test_FindInteractableObjectsAndEnableCollidersOnThem()
    {
        var bench = driver.FindObject(By.NAME, "Bench Decorated");
        bench.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", true, "UnityEngine.PhysicsModule");
        var scull = driver.FindObject(By.NAME, "Scull Candle");
        scull.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", true, "UnityEngine.PhysicsModule");
        var shrine = driver.FindObject(By.NAME, "Shrine");
        shrine.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", true, "UnityEngine.PhysicsModule");
        var plaque = driver.FindObject(By.NAME, "Plaque Candles");
        plaque.SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", true, "UnityEngine.PhysicsModule");
        var shrineWithCandles = driver.FindObjects(By.NAME, "Shrine Candles");

        for (int i = 0; i < shrineWithCandles.Count; i++)
        {
            shrineWithCandles[i].SetComponentProperty("UnityEngine.BoxCollider",
            "enabled", true, "UnityEngine.PhysicsModule");
        }
    }

    [Test, Order(15)]
    public void Test_FindSecretGameObjectAndClickOnThem()
    {
        var shrine = driver.FindObject(By.NAME, "Shrine");
        var shrinePosition = shrine.GetScreenPosition();
        var count = 10;
        var interval = 0.5f;

        driver.Tap(shrinePosition, count, interval);

        Assert.NotNull(shrine);
    }

    [Test, Order(16)]
    public void Test_WaitForSecretWindowAndCheckText()
    {
        var window = driver.WaitForObject(By.NAME, "Secret Message Text");
        var windowText = window.GetText();

        Assert.AreEqual(SecretText, windowText);
    }

    [Test, Order(17)]
    public void Test_FindPlaqueCandlesAndClickOnThem()
    {
        var plaque = driver.FindObject(By.NAME, "Plaque Candles");
        var plaquePosition = plaque.GetScreenPosition();
        var plaqueLight = plaque.FindObjectFromObject(By.NAME, "Point Light");
        driver.Tap(plaquePosition, 1);

        var activeSelf = plaqueLight.WaitForComponentProperty<bool>("UnityEngine.Transform",
            "gameObject.activeSelf", false, "UnityEngine.CoreModule");

        Assert.IsFalse(activeSelf);
    }

    [Test, Order(18)]
    public void Test_FindPauseButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Pause Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(19)]
    public void Test_CheckPausedGameTimeScale()
    {
        var value = 0.0f;
        var time = driver.GetTimeScale();

        Assert.AreEqual(value, time);
    }

    [Test, Order(20)]
    public void Test_FindContinueButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Continue Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(21)]
    public void Test_CheckCurrentTimeScale()
    {
        var value = 1.0f;
        var time = driver.GetTimeScale();

        Assert.AreEqual(value, time);
    }

    [Test, Order(22)]
    public void Test_FindPauseButtonAndClickOnThemAgain()
    {
        var button = driver.FindObject(By.NAME, "Pause Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(23)]
    public void Test_FindMusicButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Music Controller Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(24)]
    public void Test_FindAudioSourceAndCheckIsPlayingPropertyState()
    {
        var audioSource = driver.FindObject(By.NAME, "Audio Source");
        var isPlaying = audioSource.WaitForComponentProperty<bool>("UnityEngine.AudioSource",
            "isPlaying", false, "UnityEngine.AudioModule");

        Assert.IsFalse(isPlaying);
    }

    [Test, Order(25)]
    public void Test_FindSoundButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Sound Controller Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(26)]
    public void Test_FindSoundManagerAndCheckMutePropertyState()
    {
        var audioSource = driver.FindObject(By.NAME, "Sound Manager");
        var isMuted = audioSource.WaitForComponentProperty<bool>("UnityEngine.AudioSource",
            "mute", true, "UnityEngine.AudioModule");

        Assert.IsTrue(isMuted);
    }

    [Test, Order(27)]
    public void Test_FindExitButtonAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Exit Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(28)]
    public void Test_WaitForMainMenuSceneToBeLoadedAndCheckSceneName()
    {
        driver.WaitForCurrentSceneToBe(MainMenuSceneName);

        var name = driver.GetCurrentScene();

        Assert.AreEqual(MainMenuSceneName, name);
    }

    [Test, Order(29)]
    public void Test_FindMusicButtonOnMainMenuSceneAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Music Controller Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(30)]
    public void Test_FindAudioSourceOnMainMenuSceneAndCheckIsPlayingPropertyState()
    {
        var audioSource = driver.FindObject(By.NAME, "Audio Source");
        var isPlaying = audioSource.WaitForComponentProperty<bool>("UnityEngine.AudioSource",
            "isPlaying", false, "UnityEngine.AudioModule");

        Assert.IsFalse(isPlaying);
    }

#if UNITY_ANDROID
    [Test, Order(31)]
    public void Test_FindBackButtonAndClickOnThem()
    {
        driver.PressKey(AltKeyCode.Escape, 1);

        var panel = driver.FindObject(By.NAME, "Exit Game Panel");

        Assert.IsTrue(panel.enabled);
    }

    [Test, Order(32)]
    public void Test_FindContinueButtonOnMainMenuSceneAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Continue Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }

    [Test, Order(33)]
    public void Test_FindBackButtonAndClickOnThemAgain()
    {
        driver.PressKey(AltKeyCode.Escape, 1);

        var panel = driver.FindObject(By.NAME, "Exit Game Panel");

        Assert.IsTrue(panel.enabled);
    }

    [Test, Order(34)]
    public void Test_FindExitButtonOnMainMenuSceneAndClickOnThem()
    {
        var button = driver.FindObject(By.NAME, "Exit Button");
        var buttonPosition = button.GetScreenPosition();

        driver.Tap(buttonPosition, 1);

        Assert.NotNull(button);
    }
#endif

    #endregion
}
