J'ai toujours ces warnings :
Severity	Code	Description	Project	File	Line	Suppression State	Details
Warning (active)	MSB3277	Found conflicts between different versions of "WindowsBase" that could not be resolved.
There was a conflict between "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" and "WindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35".
    "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" was chosen because it was primary and "WindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" was not.
    References which depend on "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" [C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.25\ref\net8.0\WindowsBase.dll].
        C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.25\ref\net8.0\WindowsBase.dll
          Project file item includes which caused reference "C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.25\ref\net8.0\WindowsBase.dll".
            C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.25\ref/net8.0/WindowsBase.dll
    References which depend on or have been unified to "WindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" [].
        C:\Users\ALLANIJU\.nuget\packages\microsoft.web.webview2\1.0.3912.50\lib_manual\net5.0-windows10.0.17763.0\Microsoft.Web.WebView2.Wpf.dll
          Project file item includes which caused reference "C:\Users\ALLANIJU\.nuget\packages\microsoft.web.webview2\1.0.3912.50\lib_manual\net5.0-windows10.0.17763.0\Microsoft.Web.WebView2.Wpf.dll".
            C:\Users\ALLANIJU\.nuget\packages\microsoft.web.webview2\1.0.3912.50\buildTransitive\..\\lib_manual\net5.0-windows10.0.17763.0\Microsoft.Web.WebView2.Wpf.dll	DuelDeGateaux	C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets	2424		
Warning (active)	CS8604	Possible null reference argument for parameter 'sender' in 'void MainForm.PictureBox_DragDrop(object sender, DragEventArgs e, bool isHeader)'.	DuelDeGateaux	Concours de gâteau\Forms\MainForm.cs	109		
Warning (active)	CS8604	Possible null reference argument for parameter 'sender' in 'void MainForm.PictureBox_DragDrop(object sender, DragEventArgs e, bool isHeader)'.	DuelDeGateaux	Concours de gâteau\Forms\MainForm.cs	110		
Warning (active)	CS8620	Argument of type '(string, Action?)' cannot be used for parameter 'items' of type '(string text, Action onClick)' in 'ContextMenuStrip MainForm.BuildMenu(params (string text, Action onClick)[] items)' due to differences in the nullability of reference types.	DuelDeGateaux	Concours de gâteau\Forms\MainForm.cs	258		
Warning (active)	CS8620	Argument of type '(string, Action?)' cannot be used for parameter 'items' of type '(string text, Action onClick)' in 'ContextMenuStrip MainForm.BuildMenu(params (string text, Action onClick)[] items)' due to differences in the nullability of reference types.	DuelDeGateaux	Concours de gâteau\Forms\MainForm.cs	320		
Warning (active)	CS8618	Non-nullable field 'pnlTop' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the field as nullable.	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	29		
Warning (active)	CS8618	Non-nullable field 'btnNextSeason' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the field as nullable.	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	29		
Warning (active)	CS8618	Non-nullable field 'cmbSeasons' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the field as nullable.	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	29		
Warning (active)	CS8618	Non-nullable field 'webViewTournament' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the field as nullable.	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	29		
Warning (active)	CS8622	Nullability of reference types in type of parameter 'sender' of 'void TournamentForm.CmbSeasons_SelectedIndexChanged(object sender, EventArgs e)' doesn't match the target delegate 'EventHandler' (possibly because of nullability attributes).	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	54		
Warning (active)	CS8622	Nullability of reference types in type of parameter 'sender' of 'void TournamentForm.BtnNextSeason_Click(object sender, EventArgs e)' doesn't match the target delegate 'EventHandler' (possibly because of nullability attributes).	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	59		
Warning (active)	CS8622	Nullability of reference types in type of parameter 'sender' of 'void TournamentForm.WebViewTournament_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)' doesn't match the target delegate 'EventHandler<CoreWebView2WebMessageReceivedEventArgs>' (possibly because of nullability attributes).	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	66		
Warning (active)	CS8601	Possible null reference assignment.	DuelDeGateaux	Concours de gâteau\Forms\TournamentForm.cs	76		
Warning (active)	CS8618	Non-nullable property 'SelectedWinner' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the property as nullable.	DuelDeGateaux	Concours de gâteau\Forms\WinnerSelectionForm.cs	15		



Erreur quand je quitte la fenêtre de chargement de l'arbre de tournoi avant la fin de son chargement plus rapidement que prévu.
System.Runtime.InteropServices.COMException
  HResult=0x80004004
  Message=Opération abandonnée (0x80004004 (E_ABORT))
  Source=System.Private.CoreLib
  StackTrace:
   at System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(Int32 errorCode)
   at Microsoft.Web.WebView2.Core.CoreWebView2Environment.<CreateCoreWebView2ControllerAsync>d__15.MoveNext()
   at Microsoft.Web.WebView2.WinForms.WebView2.<InitCoreWebView2Async>d__25.MoveNext()
   at DuelDeGateaux.Forms.TournamentForm.<InitializeWebViewAsync>d__12.MoveNext() in Concours de gâteau\Forms\TournamentForm.cs:line 99

Globalement des lenteurs sur le chargement de la page du tournoi. C'est assez long.