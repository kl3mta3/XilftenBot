# **XilftenBot**

XilftenBot is a Discord bot used to allow the ability to communicate with Sonarr and Radarr API for the purpose of adding shows and movies.
It was created with the intent to allow users of a discord server to have the ability to add media, without having to provide them access to Sonarr or Radarr, which can be risky.
At it's core the goal is to allow easy access for adding media, while avoiding risk or damage to the Databases by uneducated users. 

**XilftenBot _CAN NOT Delete, Edit, or Move_ any existing media. 
Adding other Fuctions not included can dangerous and Not Intended or Recommended. Do at your own risk.

> [!tip]
> Always use the Sonarr and Radarr WebUI for modifying Existing media.
<br/>
<br/>

## **How to Use**
Getting XilftenBot running is straight forward and easy.
You only Need Sonarr, or Radarr or both, and a Discord bot registered with Discord(We will discuss this later)
This guide assumes you have Sonarr and Radarr installed and accessable outside of the localhost.

> [!Note]
> You can use localHost. You just have to ensure XilftenBot runs on the same machine as Sonarr and Radarr.
<br/>

If you do not have Sonarr installed Documents and download can be found at http://Sonarr.tv .
If you do not have Radarr installed Documents and download can be found at http://Radarr.video .

XilftenBot is a preProgramed Discord bot, and needs a Bot Token provided by Discord you can get one at http://discord.com/developers .
A guide to creating it can be found [Here](#-create-c-discord-bot)

### **Installation:**
Download the zip or complie and build XilftenBot.

Inside the folder you will find a file named "XilftenBot.dll.config".
Open the file in notepad or your editor of choice. 

Inside you will find the following config file. We need to fill this in to Configure XilftenBot to our Discored, Sonarr and Radarr servers.
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    
    <!-- This is your Bots Discord Token Example: "qqwe7q6w7e6qw7e67w6eq7.kjhkjn2kj31k23jn1k2j3nkDawda"-->
    <!-- You can get this when you create and register a bot at https://discord.com/developers/-->
      <add key="Token" value="" />

    <!-- This is your Guilds Id Example: "123456789876"-->
    <!-- You can get this by right clicking the Guild(server) at the top, in Discord and clicking copy Server ID-->
    <add key="GuildId" value="" />

    <!-- Your External Sonarr IP. Example: "http://123.456.7.8:1234/" -->
      <add key="SonarrIP" value="" />

    <!-- Your Sonarr API Key. You can get this from Sonarr Settings/General/Security -->
      <add key="SonarrAPIKey" value="" />

    <!-- Your External Radarr IP. Example: "http://123.456.7.8:1234/" -->
      <add key="RadarrIP" value="" />

    <!-- Your Radarr API Key. You can get this from Radarr Settings/General/Security -->
      <add key="RadarrAPIKey" value="" />
  
  </appSettings>
</configuration>

```
You can see we need 6 pieces of Information to fully Configure the bot. 

1. Discord Token is provided when you create the bot on Discord.com/Developers 


## **Create A Discord Bot**

Step 1: Log into your Discord account and click on “Advanced” in the sidebar. Then activate “Developer Mode”. Then click on “Discord API”.

![how-to-activate-developer-mode](https://github.com/kl3mta3/XilftenBot/assets/15388851/0ba1c4b1-a314-4449-814a-776ba3022ad6)

<sub>Activate “Developer Mode” in your Discord account and then click on “Discord API”<sub/>
<br/>

Step 2: In the Developer Portal, click on “Applications” in the sidebar on the left. Log in to your account again and then click on “New Application”.

![New-application](https://github.com/kl3mta3/XilftenBot/assets/15388851/951f6e4a-05b5-4d91-86e3-cd207207b4f5)

<sub>Click on “Applications” and then “New Application”.<sub/>
<br/>
Step 3: Give your bot a name and then click on “Create”.

![step 3](https://github.com/kl3mta3/XilftenBot/assets/15388851/e2d40029-504c-40a1-9426-fd2292794984)

<sub>Choose a name for your bot and then click “Create”<sub/>
<br/>

Step 4: Click on “Bot” in the left sidebar, then click on “Add Bot” to create a bot account and bot token. Take note of the token, as this is the how the bot communicates with the API.

![step 4](https://github.com/kl3mta3/XilftenBot/assets/15388851/cb658493-1449-4461-9527-dc0ade4a066e)

<sub><sub/>
<br/>

Step 5: Now it’s time to start programming your bot. Since this involves advanced programming steps, you should have previous knowledge of programming languages and tools. Use an IDEA programming environment, programming tools like Python 3 or node.js, or a text editor like Notepad++. After you’ve written the bot, save the bot file.

![step 5](https://github.com/kl3mta3/XilftenBot/assets/15388851/04716aad-2b23-47b8-a39b-393603a0ac19)

<sub><sub/>
<br/>

Step 6: Back in Developer Mode in Discord, go to “General Information” and set details like the description and app icon.

![step 6](https://github.com/kl3mta3/XilftenBot/assets/15388851/7e8bc0eb-ece8-4721-8c30-824cd1765ed2)

<sub/><sub/>
<br/>

Step 7: Go to “OAuth2” and in the field “Scopes” check the box for “bot”. Then set the permissions for your Discord bot.

![step 7](https://github.com/kl3mta3/XilftenBot/assets/15388851/1cec5df8-8125-411b-b0a8-6d480f157570)

<sub><sub/>
<br/>

///include permissions needed for users here

Step 8: The authentication link including client ID should look as follows:


<sub><sub/>
<br/>


Step 9: Select your Discord server in order to add the bot to your server.
<br/>

Commands Available:
/TV SeriesName (Year)  Searches for a Series.
/Movie MovieName (Year)  Searches for a Movie.
/Help Provides a Pm with all the Commands.

Available for Admins:

