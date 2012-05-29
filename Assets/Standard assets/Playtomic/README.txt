Playtomic iOS API
-------------------------------------------------------------------------
This file is part of the official Playtomic API for Unity games for web
and mobile.  Playtomic is a real time analytics platform for 
casual games  and services that go in casual games.  If you haven't used it 
before check it out:

http://playtomic.com/

Created by ben at the above domain on 2/25/11.
Copyright 2011 Playtomic LLC. All rights reserved.

Documentation is available at:

http://playtomic.com/api/unity

PLEASE NOTE:
You may modify this SDK if you wish but be kind to our servers.  Be
careful about modifying the analytics stuff as it may give you 
borked reports.

If you make any awesome improvements feel free to let us know!

-------------------------------------------------------------------------
GENERAL NOTES
-------------------------------------------------------------------------
Don't use crazy characters in your metric, level, leaderboard table etc 
names.  A very good idea is to use the same naming conventions for variables:

    - alphanumeric
    - keep them short (50 chars is the max)

Play time, country and source information is handled automatically, you do not 
need to do anything to collect that data.


Begin by logging a view which initializes the API, and then log any metrics you 
want.  Play time, repeat visitors etc are handled automatically.

-------------------------------------------------------------------------
Setting up
-------------------------------------------------------------------------
Add the Playtomic.unitypackage to your project

-------------------------------------------------------------------------
Initialize
-------------------------------------------------------------------------
Get your credentials from the Playtomic dashboard (add or select game then go to API page)

Playtomic.Initialize(xxx, "xxxxxxxxxxx");

-------------------------------------------------------------------------
Logging a view
-------------------------------------------------------------------------
Call this at the very start of your game loading or when a player resumes

	Playtomic.Log.View();

This registers that the player has viewed your game.  You can track when 
the player actually starts playing (eg play button) by calling this 0 or 
more times in your game:

	Playtomic.Log.Play();


-------------------------------------------------------------------------
CUSTOM METRICS
-------------------------------------------------------------------------

Custom metrics can track any general events you want to follow, like 
tracking who views your credits screen or anything else.

Call this to log any custom data you want to track.

Playtomic.Log.CustomMetric("name");
Playtomic.Log.CustomMetric("name", "group")
Playtomic.Log.CustomMetric("name", "group", unique);
	
	(string) name = your metric name, eg "ViewedCredits"
	(string) group = optional, specify the group name for sorting in reports
	(bool) unique = optional, only count unique occurrences 

-------------------------------------------------------------------------
HEATMAPS
-------------------------------------------------------------------------

Heatmaps can provide awesome visualizations for areas players are focused
on or struggling with - where they're clicking, where they're dying, the 
paths they're taking and more.

Playtomic.Log.Heatmap("metric", "heatmap", x, y);

	(string) metric = your metric name, eg "Died"
	(string) heatmap = the heatmap this belongs to, eg "Level1"
	(int) x = the x coordinate
	(int) y = the y coordinate

-------------------------------------------------------------------------
LEVEL METRICS
-------------------------------------------------------------------------

Level metrics can identify problems players are having with your difficulty 
or anything else by logging information like how many people begin vs. finish
each level.  They can help you identify major problems in your player retention.

These methods log the 3 types of level metrics - counters, ranged-value 
and average-value metrics.

- Counter metrics count how many times an event occurs across levels
- Ranged-value metrics track multiple values across a single event across levels
- Average-value metrics track the average of something across levels

Playtomic.Log.LevelCounterMetric(name, levelname);
Playtomic.Log.LevelCounterMetric(name, levelname, unique);
Playtomic.Log.LevelCounterMetric(name, level_number);
Playtomic.Log.LevelCounterMetric(name, level_number, unique);

	(string) name = your metric name
	(string) or (int) levelname / levelnumber = either a 
		level number (int > 0) or a level name
	(bool) unique = optional, only count unique-per-view occurrences

Playtomic.Log.LevelAverageMetric(name, level_name_or_number, value);
Playtomic.Log.LevelAverageMetric(name, level_name_or_number, value, unique);

	(string) name = your metric name
	(string) or (int) levelname / levelnumber = either a 
		level number (int > 0) or a level name
	(bool) unique = optional, only count unique-per-view occurrences

Playtomic.Log.LevelRangedMetric(name, level_name_or_number, value);
Playtomic.Log.LevelRangedMetric(name, level_name_or_number, value, unique);

	(string) name = your metric name
	(string) or (int) levelname / level number = either a 
		level number (int > 0) or a level name
	(int) value = the value you want to track
	(bool) unique = optional, only count unique-per-view occurrences 

-------------------------------------------------------------------------
LEADERBOARDS, LEVEL SHARING, DATA AND GEOIP
-------------------------------------------------------------------------
This stuff gets a little more complicated.  Please check the documentation at
http://playtomic.com/api/unity
