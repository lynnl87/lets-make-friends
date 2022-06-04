# lets-make-friends
Purpose of this project was to have a quick way to grab a random piece of advice, text, quote, etc to give to other players in a game.<br />
Ctrl+F12 will grab a piece of advice and 1) Copy it to the clipboard and 2) Output to the log on the screen.

#config example
```json
{
  "apis": [
    {
      "name": "Advice API!",
      "description": "Gives random words of wisdom.",
      "endpoint": "https://api.adviceslip.com/advice",
      "format": {
        "slip": {
          "id": "number",
          "advice": "string"
        }
      },
      "desiredValue": "slip.advice"
    }
    ]
}
```
| Property | Description
| :---: | :---: |
| apis | json array of objects describing the API to consume.
| name | The name to display on the ui for the API.
| description | Generalized description of the API.
| endpoint | Endpoint address to call to get the data.
| format | A json object matching the expected result json from the endpoint.
| desiredValue | Period separated value to navigate the result.
