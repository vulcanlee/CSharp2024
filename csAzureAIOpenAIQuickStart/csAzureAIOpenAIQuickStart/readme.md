# 如何將 API Key 儲存到該電腦的環境變數內，重新啟動電腦後，也需要存在；另外，刪除的時候，要下甚麼指令

要將 API Key 儲存到電腦的環境變數中，並在重新啟動後保持存在，以及如何刪除這個環境變數，步驟會根據您的作業系統而有所不同。我將為 Windows 和 macOS/Linux 系統提供說明：

## Windows 系統 (CLI 操作)

當然，我可以為您提供在 Windows 平台上使用命令行界面（CLI）來設置、查看和刪除環境變數的方法。這些操作可以通過 Command Prompt（命令提示符）或 PowerShell 完成。

以下是相關的 CLI 指令：

1. 設置環境變數：

使用 `setx` 命令來創建或更新系統環境變數：

```
setx AZURE_OPENAI_API_KEY "your_api_key_here" /M
```

注意：
- `/M` 參數表示這是一個系統環境變數（而不是用戶環境變數）。
- 這個變更會在新開啟的命令提示符中生效。

2. 查看環境變數：

要檢查環境變數是否成功設置，可以使用：

```
echo %AZURE_OPENAI_API_KEY%
```

或者在 PowerShell 中：

```
$env:AZURE_OPENAI_API_KEY
```

3. 刪除環境變數：

要刪除環境變數，可以將其值設為空字符串：

```
setx AZURE_OPENAI_API_KEY "" /M
```

這實際上並不會完全刪除變數，而是將其值設為空。如果您想完全移除變數，可以使用以下 PowerShell 命令：

```
[Environment]::SetEnvironmentVariable("AZURE_OPENAI_API_KEY", $null, "Machine")
```

4. 確認刪除：

使用之前的查看命令確認變數已被刪除或清空。

注意事項：
- 使用 `setx` 命令設置的變數更改不會立即在當前的命令提示符會話中生效。您需要開啟一個新的命令提示符窗口來看到更改。
- 如果您需要在當前會話中立即使用新設置的變數，可以使用 `set` 命令（但這只會在當前會話中有效）：

  ```
  set AZURE_OPENAI_API_KEY=your_api_key_here
  ```

- 使用 CLI 操作系統環境變數時，請確保您的命令提示符或 PowerShell 是以管理員權限運行的。

這些命令使您能夠通過 CLI 在 Windows 平台上管理環境變數，包括設置、查看和刪除 API Key。請記住，處理 API Key 等敏感信息時要格外小心，確保不要在共享或不安全的環境中暴露這些信息。

需要我進一步解釋任何部分嗎？

## Windows 系統 (GUI 操作)

1. 儲存環境變數：
a. 按 Win + R，輸入 "sysdm.cpl"，按確定開啟系統屬性。
b. 切換到「進階」標籤，點擊「環境變數」。
c. 在「系統變數」區域，點擊「新增」。
d. 變數名稱輸入如 "AZURE_OPENAI_API_KEY"，變數值輸入您的 API Key。
e. 點擊確定保存。

2. 刪除環境變數：
- 在命令提示字元（CMD）中輸入：
```
setx AZURE_OPENAI_API_KEY ""
```
- 然後刪除系統變數：重複步驟 1a-1b，在系統變數列表中找到 AZURE_OPENAI_API_KEY，選中並點擊「刪除」。

## macOS/Linux 系統

1. 儲存環境變數：
a. 打開終端。
b. 編輯 ~/.bash_profile 文件（對於 macOS）或 ~/.bashrc 文件（對於大多數 Linux 發行版）：
```
nano ~/.bash_profile  # 對於 macOS
# 或
nano ~/.bashrc        # 對於 Linux
```
c. 添加以下行：
```
export AZURE_OPENAI_API_KEY="your_api_key_here"
```
d. 保存並退出（Ctrl+X，然後 Y，再按 Enter）。
e. 重新加載配置文件：
```
source ~/.bash_profile  # 對於 macOS
# 或
source ~/.bashrc        # 對於 Linux
```

2. 刪除環境變數：
a. 再次編輯 ~/.bash_profile 或 ~/.bashrc 文件。
b. 刪除或註釋掉包含 AZURE_OPENAI_API_KEY 的那一行。
c. 保存並退出。
d. 重新加載配置文件（如步驟 1e）。
e. 可以通過以下命令立即在當前會話中取消設置該變數：
```
unset AZURE_OPENAI_API_KEY
```

注意事項：
- 在 Windows 中，系統環境變數的更改會立即生效，但某些正在運行的應用程序可能需要重新啟動才能看到新的環境變數。
- 在 macOS/Linux 中，新的終端會話會讀取更新後的配置文件，但現有的會話需要重新加載配置或重新啟動終端。
- 請確保將 API Key 等敏感信息保管安全，不要將其分享或暴露在公開場合。

需要我詳細解釋某個步驟嗎？
