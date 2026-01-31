using System.Collections;
using UnityEngine;
using System.Linq;
public static class ResolutionSettings
{
    
#if UNITY_STANDALONE_WIN
#else
    static int _amountOfResolutions = 6;
#endif   

    public const string RESOLUTION_KEY = "Resolution";
    public const string IS_FULLSCREEN_KEY = "IsFullScreen";

    public static Resolution[] GenerateResolutions() {
#if UNITY_STANDALONE_WIN
        Resolution[] _resolutions;
#elif UNITY_WEBGL
        Resolution[] _resolutions = new Resolution[_amountOfResolutions];  
#elif UNITY_ANDROID
        Resolution[] _resolutions = new Resolution[_amountOfResolutions]; // For some reason, Screen.resolitions won't return the available resolutions for some android devices. So this has to be done.
#endif   
        #if UNITY_STANDALONE_WIN
        _resolutions = Screen.resolutions.Select(resolution => 
        new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
#elif UNITY_WEBGL
        for(int i = 0; i < _amountOfResolutions; i++) {
            _resolutions[i].height = Display.main.systemHeight*(i+1)/_amountOfResolutions;
            _resolutions[i].width = Display.main.systemWidth*(i+1)/_amountOfResolutions;
        }
#elif UNITY_ANDROID
        for(int i = 0; i < _amountOfResolutions; i++) {
            _resolutions[i].height = Display.main.systemHeight*(i+1)/_amountOfResolutions;
            _resolutions[i].width = Display.main.systemWidth*(i+1)/_amountOfResolutions;
        }
#endif   
        return _resolutions;
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        var resolutions = GenerateResolutions();
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++) {
            if(resolutions[i].width == currentWidth && resolutions[i].height == currentHeight) currentResolutionIndex = i;
        }

        SetResolution(PlayerPrefs.GetInt(RESOLUTION_KEY, currentResolutionIndex));
        SetFullScreen(PlayerPrefs.GetInt(IS_FULLSCREEN_KEY, 1) == 1);
    }

    
    public static void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(IS_FULLSCREEN_KEY, isFullScreen ? 1 : 0);
    }
    public static bool IsFullscreen() => PlayerPrefs.GetInt(IS_FULLSCREEN_KEY, 1) == 1;
    public static void SetResolution(int resolutionIndex) {
        Resolution resolution = GenerateResolutions()[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_KEY, resolutionIndex);
    }


}
