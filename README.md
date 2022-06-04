# lets-make-friends
Purpose of this project was to query a RESTful advice api to give me ...advice to give to other players.
Ctrl+F12 will grab a piece of advice and 1) Copy it to the clipboard and 2) Output to the log on the screen.

#config example
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