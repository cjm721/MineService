# MineService
Server Administration Software, Currently Focused around Minecraft.

This was originaly a project for CSSE-371 (Software Requirements) and then was also used later in CSSE-375 (Software Construction and Evolution).
There are alot of features done but there are many more that could be expanded.

## Features
* Start / Stop of servers
* Read / Write to each server console
* Some Settings for each server
* Perfomance monitoring

## Client
Written in C# using Windows Forms

## Server
C# Version is up to date. There is also a Java Version that works but not with all current features.

## Communication
Uses JSON to seralizaize messages on the network stream. On both ends the messages are translated back into objects. 
These messages also also fully encrypted using DES. Currently the Keys are Hardcoded but could be expanded to use RSA 
for inital key sharing.

## Team
### CSSE-371
* CJ Miller (Owner / Manager)
* Ishank Tandon
* Max Morgan
* Jeremy Brinegar
### CSSE-375
* CJ Miller (Owner / Manager)
* Collin Trowbridge
* Brandon Hennessey
