# DotNetTwitterBot

Hello! This is a dotnet twitter bot created to listen and retweet based on the tags and things you set via the rules API.
I've created a blog [here](https://dev.to/iamabdul/running-a-net-twitter-bot-on-pi-part-1-the-bot-2cdp) explaining some of my decisions going into this and what else I plan to do.

## To run this app
You'll need the following values you can get from your twitter developer portal. 
This will then be copied and pasted into the appsettings.Json file [here](https://github.com/Iamabdul/DotNetTwitterBot/blob/main/TwitterBot/appsettings.json).
"BearerToken": "{Bearer Token Here}" //dev portal
"BotUserId": "{Bot UserId Here}" // got to https://tweeterid.com/ to get this
"ApiKey": "{Bot ApiKey Here}" //dev portal
"ApiSecret": "{Bot ApiSecret Here}" //dev portal
"AccessToken": "{Bot AccessToken Here}" //dev portal
"AccessTokenSecret": "{Bot AccessTokenSecret Here}" //dev portal

## Pre condition
In order for the listener to start listening to things, rules need to be set out first.
Depending on when you'll see this, I'll be able to add this functionality to this bot so you don't have to set it up outside of this app.
Just incase you need to know how to set up the rules, you can download the postman collection from twitter and follow the instructions. 
It will only need ot be set once throughout the life of the account, so no worries on having to go back and forth 👍.
