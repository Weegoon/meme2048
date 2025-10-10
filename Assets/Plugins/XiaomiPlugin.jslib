var functions = {
	XiaomiSDK_ShowInterstitial : function (){
		adBreak({
        type: 'next',
        name: 'next',
        beforeAd: () => { myInstance.SendMessage('XiaomiServices', 'OnPauseGame'); },  
        afterAd: () => { myInstance.SendMessage('XiaomiServices', 'OnResumeGame'); },
        adBreakDone: (placementInfo) => { myInstance.SendMessage('XiaomiServices', 'OnResumeGame'); },
      });
	},
  XiaomiSDK_ShowFirstInterstitial : function (){
    adBreak({
        type: 'preroll',
        name: 'preroll',
        adBreakDone: (placementInfo) => { console.log(placementInfo); },
      });
  },
  XiaomiSDK_ShowReward : function (){
    adBreak({
      type: 'reward',  // rewarded ad
      name: 'reward',   
      beforeReward: (showAdFn) => {
        showAdFn();
      },  
      beforeAd: () => { myInstance.SendMessage('XiaomiServices', 'OnPauseGame'); },
      adDismissed: () => {
        myInstance.SendMessage('XiaomiServices', 'OnResumeGame');
      },
      adViewed: () => { 
        myInstance.SendMessage('XiaomiServices', 'OnResumeGame');
        myInstance.SendMessage('XiaomiServices', 'OnRewardedGame');
       }, // Reward granted
      afterAd: () => { myInstance.SendMessage('XiaomiServices', 'OnResumeGame'); }, // Resume the game flow.
    });
  },
  XiaomiSDK_LoadReady : function (){
    xiaomiLoadReady();
  }
};

mergeInto(LibraryManager.library, functions);