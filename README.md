# STIB

The goal of this project is to allow computer organization students to use their aquired knowledge about curcuits and logic gates to engage in a fun and engaging way. STIB (bits backwards) is a game to allow students to explore the inner workings of a computer without the preasure of a classroom. 


## OurProcess

- Research
- Movement
- Block System


## Research

We were inspired by youtubers who were able to make entire computers with even a UI system with minecraft redstone. 
https://www.youtube.com/watch?v=CW9N6kGbu2I
https://www.youtube.com/watch?v=-BP7DhHTU-I
There are problems with redstone though, it doesn't work infinitenly, and making simple and / or gates require way too many components. So we decided to make a better system. To do this we needed to think about how we wanted to update the blocks. And to do it justice we must be able to do the following things:
- circuits must be able to update quickly and instantly (within one tick)
- the system must be able to support an infinite world
- the basic logic must be easy to understand and the basic components (the logic gate's ui) must be small to make it expandable

## Movement

To allow for an infinite 3D world we need:
- fast and slow speeds
- wasd movement (q and e for z axis movement)

## Block System

We are currently implementing these blocks
- and
- or
- wire
- Not
- send
- LED