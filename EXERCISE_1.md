# Question Answers #

## How can your implementation be optimized? ##

My method to search for the nearest item and/or bot is a simple bubble sort. There are other sorts that could make this more optimal, but I felt the real solution was to implement a data structure such as a KD-Tree to store items/bots. While I understand the idea behind how a KD-Tree organizes data, and how that would make the search more efficient when starting to hit an item/bot count in the thousands, I haven’t written one out before. 

I decided to stick with the bubble sort because it works well enough for up to a couple hundred bots/items, and I could focus the rest of my time on ensuring I did everything else that was asked. If I was to implement a KD-Tree, I would have spent my whole time making that. Alternatively I could have implemented one I found that uses an MIT license; however, that wouldn’t be my code I don’t want to run afoul of this exercise. Given more time, I think writing a proper KD-Tree to store my items/bots instead of a List is where I would focus to improve performance when scaling to large quantities.

## How much time did you spend on your implementation? ##

In total, I set aside about 16 hours for this. The breakdown of that is around 8 hours of programming, 5 hours of researching/refactoring/contemplating existence, and another 3 hours lost to pictures of cats.

## What was most challenging for you? ##

Honestly, second guessing myself was a challenge. In an effort to show everything I know about separation of concerns, designing with patterns, using contracts (interfaces), and generally demonstrating solid Comp Sci skills, I worried I’d over engineered it. That may be the case for how simple the functionality of this exercise is; however, I stand by my structure because I feel it would scale well if it were to become something more. That is, I feel confident I have an easy path to add more functionality, to troubleshoot, and to generally reuse the most code with the least amount of spaghetti code.

## What else would you add to this exercise? ##

It was pretty good as it is. I see where an individual could solve this a myriad of different ways. If I were to add anything to it, it would be a goal. Give the play/user some obstacle to overcome and/or accomplishment to achieve. Make it feel like a real gameplay, or at least interaction loop.

—- Original Instructions —-

# Exercise 1 #

In this exercise you'll configure a Unity scene and write scripts to create an interactive experience. As you progress through the steps, feel free to add comments to the code about *why* you choose to do things a certain way. Add comments if you felt like there's a better, but more time intensive way to implement specific functionality. It's OK to be more verbose in your comments than typical, to give us a better idea of your thoughts when writing the code.

## What you need ##

* Unity 2020 (latest, or whatever you have already)
* IDE of your choice
* Git

## Instructions ##

This test is broken into multiple phases. You can implement one phase at a time or all phases at once, whatever you find to be best for you.

### Phase 1 ###

**Project setup**:

 1. Create a new Unity project inside this directory, put "Virbela" and your name in the project name.
 1. Configure the scene:
     1. Add a central object named "Player"
     1. Add 5 objects named "Item", randomly distributed around the central object
 1. Add two C# scripts named "Player" and "Item" to your project
     1. Attach the scripts to the objects in the scene according to their name, Item script goes on Item objects, Player script goes on Player object.
     1. You may use these scripts or ignore them when pursuing the Functional Goals, the choice is yours. You're free to add any additional scripts you require to meet the functional goals.

**Functional Goal 1**:

When the game is running, make the Item closest to Player turn red. One and only one Item is red at a time. Ensure that when Player is moved around in the scene manually (by dragging the object in the scene view), the closest Item is always red.

### Phase 2 ###

**Project modification**:

 1. Add 5 objects randomly distributed around the central object with the name "Bot"
 1. Add a C# script named "Bot" to your project.
 1. Attach the "Bot" script to the 5 new objects.
     1. Again, you may use this script or ignore it when pursing the Functional Goals.

**Functional Goal 2**:

When the game is running, make the Bot closest to the Player turn blue. One and only one object (Item or Bot) has its color changed at a time. Ensure that when Player is moved around in the scene manually (by dragging the object in the scene view), the closest Item is red or the closest Bot is blue.

### Phase 3 ###

**Functional Goal 3**:

Ensure the scripts can handle any number of Items and Bots.

**Functional Goal 4**:

Allow the designer to choose the base color and highlight color for Items/Bots at edit time.

## Questions ##

 1. How can your implementation be optimized?
 1. How much time did you spend on your implementation?
 1. What was most challenging for you?
 1. What else would you add to this exercise?

## Optional ##

* Add Unit Tests
* Add XML docs
* Optimize finding nearest
* Add new Items/Bots automatically on key press
* Read/write Item/Bot/Player state to a file and restore on launch
* Restructure your code to leverage [SOLID](https://en.wikipedia.org/wiki/SOLID) principles. (comment and tag any revisions for this)

## Next Steps ##

* Confirm you've addressed the functional goals
* Answer the questions above by adding them to this file
* Commit and push the entire repository, with your completed project, back into a repository host of your choice (bitbucket, github, gitlab, etc.)
* Share your project URL with your Virbela contact (Recruiter or Hiring Manager)

## If you have questions ##

* Reach out to your Virbela contact (Recruiter or Hiring Manager)
