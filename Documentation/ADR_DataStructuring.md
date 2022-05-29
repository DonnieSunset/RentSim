# Data Structures

## Context and Problem Statement

How to structure data?
There are kind of different data in the application:
* Input Data
* Result Data
* Data for the three different phases

## Considered Options

* Flat data structure 
* Separation between LifeAssumptions and MarketAssumptions
* Separate in 
    * WorkPhase Input / Result Data
    * StopWorkPhase Input / Result Data
    * RentPhase Input / Result Data

## Decision Outcome

There must be at least a separation between input data and result data, because we want to create different defaults later for the input to cover different scenarios.
But shall we also separate in different Phases?