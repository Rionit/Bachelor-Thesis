### Prerekvizity pro Build

- Unity verze **2022.3.48f1**. 
- Ověřte, že máte nainstalované potřebné moduly (**Android Build Support, iOS Build Support**). 
- **Android** nebo **iOS** telefon
- macOS zařízení s aplikací **Xcode** (v případě iOS)

### Postup Buildu

1. Otevřete projekt.
2. Přejděte do *File > Build Settings*. 
	1. Vyberte platformu **Android** nebo **iOS** a klikněte na **Switch Platform**. Zkontrolujte, že je zaškrtnutá pouze scéna *Scenes/Main*.

#### Android

1. V případě **Androidu** stačí stisknout **Build** a vhodně aplikační soubor pojmenovat. 
2. Následně přeneste tento .apk soubor do vašeho Android zařízení a nainstalujte jej. 
3. Při spuštění aplikace povolte přístup k fotoaparátu.

#### iOS

1. V případě iOS nejprve zkontrolujte, že v **Player Settings** je **Target minimum iOS version** nejméně 12.0. 
2. Následně v **Build Settings** zaškrtněte políčko **Development Build** a nastavte **Run in Xcode as** na **Release**. 
3. Poté spusťte tento vzniklý **Xcode projekt** v aplikaci **Xcode** na macOS zařízení. 
4. Připojte k tomuto zařízení Váš iPhone a zvolte jej nahoře jako cílové zařízení. 
5. Vyčkejte, než si Xcode importuje a extrahuje určitá data. 
6. Následně stiskněte šipku v levém horním rohu pro Build. 
	1. Vyskočí na Vás error, abyste nastavili certifikát. 
	2. Klikněte na **Automatically manage signing** a následně na **Team**, kde vytvořte (pokud nemáte) nový tým. Stačí se přihlásit přes Vaše Apple ID. 
7. Po následovném stisknutí tlačítka Build budete požádáni o zapnutí Developer módu ve Vašem iPhone -- postupujte prosím podle instrukcí. 
8. Nakonec budete muset v zařízení iPhone stisknout tlačítko, že důvěřujete této aplikaci. 
9. Opět při spuštění aplikace povolte přístup k fotoaparátu. 

##### Poznámky

Doporučujeme v případech, kdy nějaký krok nefunguje, nejprve vysunout a zasunout USB kabel. V případě chyby ***Command PhaseScriptExecution failed with a nonzero exit code*** v našem případě zafungovalo nejprve odškrtnout a zaškrtnout **Development Build** a opětovný Build v Unity (případně zkusit i Build "na prázdno" s odškrtnutým políčkem nebo přepnout na Android platformu a zase zpátky).

Po otevření Unity projektu doporučuji vypnout viditelnost objektu "GLB mesh".

### Ukázka renderů mračen bodů a GLB objektu budovy E

#### GLB objekt budovy E na ČVUT FEL, Karlovo náměstí 

![full_iso_view](https://github.com/user-attachments/assets/f0dcac0d-ba94-45d8-ab5d-ef6f2ae549fc)

#### Mračna bodů lokací budovy E na ČVUT FEL, Karlovo náměstí 

![full_iso_view_colored](https://github.com/user-attachments/assets/ef292e67-9bc0-43a8-b9d9-94361e37886c)

### Ukázka starého a nového UI

https://github.com/user-attachments/assets/b88583cb-5254-4be6-bdf7-ecb25861b8e8
