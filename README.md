# C# 專案範例原始碼

|專案名稱|專案說明|備註|
|-|-|-|
|* csElasticsearchNestFindIndex|如何使用 C# / NEST 找到是否有匹配符合的 index 名稱||
|* csElasticsearchNestChangeSetting|Elasticsearch 系列 : 如何使用 C# / NEST 來變更 index.max_result_window 大小||
|csBlazorTodo|使用 Blazor 設計 Todo 元件|https://reactsimpletodo.darenge.net/|
|* csBlazorSSR|.NET 8 Blazor 001 - 了解 SSR Static Server Render 運作模式||
|csExceptionToString|將 Inner, Aggregation 等所有例外異常物件，轉成字串||
|csBlazorLogin|.NET 8 Blazor Server Interactive Cookie 登入||
|mauiSettingConfiguration|||
|mauiPrismNavigationEventWatch|觀察 MAIU for Prism 的導航事件觸發時機||
|mauiMessengerCommunication|重複訂閱事件的問題修正做法||
|mauiSoftKeyboardVisibility|軟體鍵盤的隱藏控制||
|mauiSwitchScreenAlwaysOn|螢幕持續恆亮||
|mauiLockScreenPortrait|在iOS與Android裝置下，鎖定Portrait模式||
|mauiSleepLifecycle|偵測 App 是否進入到背景或休眠模式下||
|mauiAlertSnackbarToast|在 MAUI 使用警示功能 - Snackbar 與 快顯通知||
|csSymmetricEncryption|使用對稱 Symmetric Encryption 進行資料加解密||
|mauiExceptionToclipboard|MAUI 捕捉到無法預期例外異常，並將其寫入剪貼簿||
|mauiStatusBar|設定狀態列的顏色||
|mauiPopup|全螢幕彈出對話窗||
|csAsymmetricEncryption|非對稱的加解密使用方式||
|csDigitalSignature|內容數位簽名使用方式||
|mauiNoNetworkConnection|無網路下發生的閃退||
|mauiExceptionControl|整體App遇到例外異常時候的處理作法||
|* csConfigurationRunTimeChange|在執行時期能夠取得 appsettings.json 變更後的內容值||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||
||||

iOS Simulator: A fatal error occurred while trying to start the server.

xcrun simctl shutdown all

rm -r ~/Library/Developer/CoreSimulator/Caches

sudo rm -R /Users/swee/Library/Developer/CoreSimulator/Caches

open -a Simulator

cat /Library/Logs/CoreSimulator/CoreSimulator.log

On macOS 13 and above
Go to System Settings → General → Storage → Developer
Delete "Developer Caches"
