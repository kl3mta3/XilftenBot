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
Commands Available:
/TV SeriesName (Year)  Searches for a Series.
/Movie MovieName (Year)  Searches for a Movie.
/Help Provides a Pm with all the Commands.

Available for Admins:

