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

If you do not have Sonarr installed Documents and download can be found at http://Sonarr.tv.

If you do not have Radarr installed Documents and download can be found at http://Radarr.video.

XilftenBot is a preProgramed Discord bot, and needs a Bot Token provided by Discord you can get one at http://discord.com/developers.
A guide to creating it can be found [here](#create-a-discord-bot)

<br/>
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
You can see we need 6 pieces of Information to fully Configure the bot. 

1. Token is your Bots Token provided when you create the bot on Discord.com/Developers. Ex: 1231d12dasda98sdahsd.123kj12k3j12kmdk12jnb
2. GuildId id the Id of the Discord server you added to bot to. You can get this by right clicking the server name at the top, in Discord and clicking copy Server ID
3. SonarrAPI is your Sonarr API Key  and is Located in Sonarr Settings/General/Security.
4. Sonarr IP is the address in your browser when you connect to Sonarr with. Ex: http://123.456.7.8:8989/ or http://localhost:8989/
5. RadarrAPI is your Radarr API Key  and is Located in Radarr Settings/General/Security.
6. Radarr IP is the address in your browser when you connect to Radarr with. Ex: http://123.456.7.8:7878/ or http://localhost:7878/

Once we have all that entered we can start the bot.  Thats it, it is ready to use from Discord.

<br/>
<br/>

### **Using the Bot Inside Discord**

XilftenBot uses Application Commands.  Application commands come in two options Global(work on every server the bot is on) and Guild(Server) specific. 
Due to the nature of the Tasks XilftenBot performs, by default it is set to use Guild Specific Application Commands.
This ensures if you accidentally make the bot public and someone adds it to their Discord server, they can't affect Sonarr or Radarr. 

If you wish the Commands to be accessable Globally ( Work in many Servers) it can be done as follows.

Open the file Xilften.cs and locate the following code.
```
var slash = discord.UseSlashCommands();
slash.RegisterCommands<RadarrCommand>(guildID);
slash.RegisterCommands<BotCommand>(guildID);
slash.RegisterCommands<SonarrCommand>(guildID);
```

Change it to,
```
var slash = discord.UseSlashCommands();
slash.RegisterCommands<RadarrCommand>();
slash.RegisterCommands<BotCommand>();
slash.RegisterCommands<SonarrCommand>();

```
Once done, save, rebuild the project and restart the app. Now it will register the Application Commands Globally
>[!Warning]
>Global Application commands can take an hour to register with Discord, And once Registered can not be
>removed without the use of an API call or by deleting the app on Discord.com and rebuilding a new bot there.

<br/>

### **Best Discord Setting for XilftenBot**

Our Application Command including the ones requiring Admin Access are all visible. 

To better ensure only the users we want to use the commands can see them we need to change a few settings in the Discord server. 

<br/>

#### **_Roles_**

To aid in the above goal we need to create a few rolls. We Assume you already have an Admin roll with Admin access.

We need to create Two more. For this guide we will call them CanAddShows and CanAddMovies.

To create a new role, Go to the server setting by clicking the icon to the right of the server name in Discord.

Once in settings Click on Roles, then "Create Role" give the roll a name and save. 

After you have both roles made we need assign CanAddShows to anyone we want to access /TV to add shows and assign CanAddMovies to people we want to have access to /Movies.
This allows us to restrict each action to a user incase we want some poeple to only add one or the other. 

<br/>

#### **_Permissions_**

Now we need to set the permissions for the commands to all and show for only the roles we want. Head to your Servers Settings again by clicking on the icon to the right of the name. 
This time we need to click on Intergrations in the Apps section.

Here you should see your bots name  in its tab under "Bots and Apps". Click on "Manage", located on the right of your bots tab.
We need to add the 3 roles we want and give access. but we also need to remove access for @everyone so only the people we want can see and interact with the bot.

Set it as in the image below.

![Bot Permissions](https://github.com/kl3mta3/XilftenBot/assets/15388851/45488fe6-78a8-4362-a641-84d2f1af1a08)

Below this you will see a list of all the commands XilftenBot registered with Discord when it Connected.

![commands1](https://github.com/kl3mta3/XilftenBot/assets/15388851/d55224a5-7904-43d4-92c7-4a48bd509c55)

Go through each command and Set its permissions, Keep in mind 

* Only CanAddShows and Admin should have access to /TV
* Only CanAddMovie and Admin should have access to /Movies
* All Commands should have @everyone restricted.
* All Commands should have Admin Allowed.


![helpCommandPermission](https://github.com/kl3mta3/XilftenBot/assets/15388851/555be194-e03b-4275-b4b5-de0834a5e393)

 For /TV and /Movies we need to also restrict requests to the specific channel for requests. This is to keep the server clean and prevent Requests from cluttering up every channel.
 click on add channel and choose a channel.

![movieCommandPermission](https://github.com/kl3mta3/XilftenBot/assets/15388851/3c2cce7d-7d94-4bb2-9a06-ef20e5968f54)

Thats it Save your settings and Its all set up and ready for users to use. 


### **Using The Commands**

#### How to search for a series.

Go to the Request channel and Type / Discord will suggest possible commands. 
Choose Tv or type Tv and press space. Then provide a show name and ideally year in the format "SeriesName (2024)".

If the Show exists on the server you will Recieve a respons like the following. Showing the shows info. 

![tvrequestontherserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/509c7d54-eabc-478f-b649-6697a2cbd166)

If the show doesnt exist then You will receive a reponse like this.

![Shownotonserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/cc6335e9-97d8-4ac1-b126-10aa5eba0803)

Respond with the "okay" emoji :ok_hand: within 30 seconds and it will add the show to the server.

![showaddedtoserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/05957548-ac68-4dd7-b112-238a3715ede9)

<br/>

#### How to search for a movie.

Go to the Request channel and Type / Discord will suggest possible commands. 
Choose Movie or type movie and press space. Then provide a movie name and ideally year in the format "MovieName (2024)".

If the movie exists on the server you will Recieve a response like the following. Showing the movie info. 

![Movieonserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/5892d709-e7ba-49f5-ad2d-15d5ab48f401)

If the movie doesnt exist then You will receive a reponse like this.

![Movienotonserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/f6f5ee82-fe1e-4dfd-b8c9-ccaa747d81f9)

Respond with the "okay" emoji :ok_hand: within 30 seconds and it will add the movie to the server.

![moviesaddedtoserver](https://github.com/kl3mta3/XilftenBot/assets/15388851/fd99797a-44ee-421b-9aa1-8d8c09da7740)

<br/>




<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>

## Create A Discord Bot

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

<sub>Click on “Add Bot” to generate a Bot token.<sub/>
<br/>


Step 5: Back in Developer Mode in Discord, go to “General Information” and set details like the description and app icon.

![step 6](https://github.com/kl3mta3/XilftenBot/assets/15388851/7e8bc0eb-ece8-4721-8c30-824cd1765ed2)

<sub/>Under “General Information”, you can set details like a description and icon for your bot.<sub/>
<br/>

Step 6: Go to “OAuth2” and in the field “Scopes” check the box for “bot”. Then set the permissions for your Discord bot.

![step 7](https://github.com/kl3mta3/XilftenBot/assets/15388851/1cec5df8-8125-411b-b0a8-6d480f157570)

<sub>Under “OAuth2”, you can set the permission for your bot.<sub/>
<br/>

///include permissions needed for users here

Step 7: The authentication link including client ID should look as follows:

![step 8](https://github.com/kl3mta3/XilftenBot/assets/15388851/4246e62b-c538-4d7a-88b5-a564a7462a0c)

<sub>Take care of the final settings for your bot and click “Copy”.<sub/>
<br/>


Step 8: Select your Discord server in order to add the bot to your server.
<br/>

