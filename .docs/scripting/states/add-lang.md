---
order: 99
---
# Adding Dialogue Translations

In order to show any text in the game, you will need a .lang file to be included. This will allow the game to fetch different text for different locales. An example .lang file would look like this:
```json
{
  "dialog.example.1": {
    "translations": {
      "EN_US": "Hello World!"
    }
  }
}
```

You can have different tags like `<color=\"Orange\"></color>`, for example:
```html
"This white text has a <color=\"Red\">Red</color> coloured word in the middle."
```
Learn more about using colours [here](/scripting/color/)!