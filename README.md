# Vehicle-Routing-Problem

## Introduction

This repo contains a C# project with the Vehicle Routing Problem solved using the following different heuristics:
-  Greedy
-  Greedy Randomized Adaptative Search Procedure (GRASP)
-  General Variable Neighbourhood Search (GVNS).

Informally, the Vehicle Routing Problem (VRP) is a combinatorial optimization problem that asks which is 
the optimal set of routes for a fleet of vehicles that have to satisfy the demands of a given client set.

In our case, we're adding another restriction; the route for every vehicle will have a max number of clients
visited given by the following expression: 

(N/K) + 0.1N

Where N corresponds to the number of clients, and K to the number of available vehicles.

## Code description
An OO approach was used since this was a class assignment, and it was one of the requirements.

### Problem class
The problem class will tackle the task of parsing and extracting all of the data files. It will store 
the filename aswell as the number of clients and the distance matrix between every client.

### Solution class
To represent the solutions, a simple class structure is used, where "Solution" is an abstract class that
will be extended for each unique algorithm that will be implemented.

The actual solutions to the problems, will be stored in a List with K elements (Where K is the number of vehicles)
and each element will store the corresponding route for each vehicle.

## Algorithms
Three algorithms and four neighbourhood searches have been implemented:

### Greedy
"GreedyWithRCL" is the class that implements a greedy approach with a Restricted Candidate List (RCL). 
In order to solve the problem with a strict greedy approach the RCL is assigned to 1 by default.

### Greedy Randomized Adaptative Search Procedure (GRASP)
The class 'GRASP' implements a GRASP algorithm. The 'Solve' method takes as a parameter the size of the RCL (Restricted Candidate List) 
for the constructive phase, and a 'GraspType' enum used to identify the neighborhood search that will be used in the local search phase.

### General Variable Neighbourhood Search (GVNS)
The 'GVNS' class implements a GVNS algorithm. The 'Solve' method takes as a parameter the size of the RCL (Restricted Candidate List) that will be used to initialize the first solution. The Variable Neighborhood Descent (VND) process is carried out with the four implemented neighborhood searches, in the following order:

-Multi-route reinsertion: This movement involves taking elements from one route one by one and inserting them into every possible position in the rest of the routes.
-Intra-route reinsertion: For each route, this movement involves taking elements one by one and inserting them into every possible position within the same route.
-Multi-route exchange: This movement consists of performing all possible combinations of exchanges between two elements that are in different routes.
-Intra-route exchange: For each route, this movement consists of performing all possible exchanges of pairs of elements within the same route.
### Results and conclusions

#### Greedy Algorithm:
| Size  | Cost (Size) | Time   |
|-------|-------------|--------|
| 40-2  | 40 (2)      | 227 ms |
| 40-4  | 40 (4)      | 0 ms   |
| 40-6  | 40 (6)      | 0 ms   |
| 40-8  | 40 (8)      | 0 ms   |

Table 1: Best results of the Greedy algorithm.

####  GRASP Algorithms:
Results of the GRASP algorithms:
#### Multi-route reinsertion as local search
| Size  | RCL  | Cost (Size) | Time     |
|-------|------|-------------|----------|
| 40-2  | 2    | 116         | 270 ms   |
| 40-4  | 2    | 155         | 310 ms   |
| 40-6  | 2    | 190         | 295 ms   |
| 40-8  | 2    | 240         | 960 ms   |

Table 2: Best results of the GRASP algorithm with multi-route reinsertion as local search.

##### Intra-route reinsertion as local search
| Size  | RCL  | Cost (Size) | Time     |
|-------|------|-------------|----------|
| 40-2  | 2    | 128         | 249 ms   |
| 40-4  | 2    | 164         | 163 ms   |
| 40-6  | 2    | 221         | 70 ms    |
| 40-8  | 2    | 305         | 74 ms    |

Table 3: Best results of the GRASP algorithm with intra-route reinsertion as local search.

##### Multi-route exchange as local search
| Size  | RCL  | Cost (Size) | Time     |
|-------|------|-------------|----------|
| 40-2  | 2    | 149         | 99 ms    |
| 40-4  | 2    | 179         | 140 ms   |
| 40-6  | 2    | 248         | 49 ms    |
| 40-8  | 2    | 307         | 79 ms    |

Table 4: Best results of the GRASP algorithm with multi-route exchange as local search.

#### 3.3. GVNS Algorithm:
Results of the GVNS Algorithm:
| Size  | Cost (Size) | Time     |
|-------|-------------|----------|
| 40-2  | 101         | 206 ms   |
| 40-4  | 134         | 359 ms   |
| 40-6  | 171         | 158 ms   |
| 40-8  | 175         | 395 ms   |

Table 5: Best results of the GVNS algorithm.


### GVNS Algorithm

Results of the GVNS Algorithm:

| Size  | Cost (Size) | Time     |
|-------|-------------|----------|
| 40-2  | 101         | 206 ms   |
| 40-4  | 134         | 359 ms   |
| 40-6  | 171         | 158 ms   |
| 40-8  | 175         | 395 ms   |

Table 6: Best results of the GVNS algorithm.

### 4. Conclusions

Comparing the obtained results, it's clear that the GVNS algorithm is much more efficient than the others. It is capable of finding higher-quality solutions among the implemented algorithms without significantly compromising the execution time.
