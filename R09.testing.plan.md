# CSC 493: R09: Testing Plan<sup>1</sup>

## Overview
You should have a working prototype of some kind now, and it is time to start testing and adding features. 
Read [The One Page Test Plan](https://www.ministryoftesting.com/dojo/lessons/the-one-page-test-plan)

## Documentation

You have some tasks to complete before next time:
- Create a test plan document in a file called <code>test.plan.md</code>.
- Update <code>README.md</code> to link to the file you just created.
- Continue implementing pieces of your project (this assignment will be accompanied by another one on how to create issues on GitHub and linked them to commits and pulls.

<code>test.plan.md</code>.

The first section of the One Page Test Plan should be a clearly written summary of the testing activities and the overall approach to the testing.

The rest of this file focuses on Key Test Cases (which will become issues you resolve on GitHub), which each provides a specific description of the 
key interactions with the software that a tester plans to do in order to test one single behavior of the software. 

See [Project Test Plan](https://d2h1nbmw1jjnl.cloudfront.net/ckeditor/pictures/data/000/000/067/content/trello_project_plan.jpg) 
for an example of a set of key test cases created in a Trello board. Fortunately, you can do something VERY SIMILAR to this process
on GitHub, and you are welcome to ask the TA forhow to do it.

A typical key test case is usually laid out in a table or bulleted format and contains:
- A unique name and number (which may be related to the requirement number, but are typically
not identical.
- The specific requirement that this Test Case is intended to test.
- Preconditions which detail which state of the software needs to be in to exercise the Test Case
(sometimes this is a previous Test Case that must always be run before the current Test Case.)
- Steps that describe the specific steps that make up the interaction with the software to run the
Test Case
- The desired results that describe the desired state of the software after the Test Case is executed
successfully

<code>README.md</code>

You should update the <code>README.md</code> file with a link to your Test Plan.

- Project name
- Project Concept (linked to <code>concept.md</code>)
- Vision<br>
The paragraph of your vision is here.
- Scope<br>
The paragraph of your scope is here.
- Prerequisites
- Requirements (linked to <code>requirements.md</code>)
- Design (linked to <code>design.md</code>)
- Test Plan (linked to <code>test.plan.md</code>)
- Built With
- Author name
  - About the Lead Developer
  Your biographical information 
- Acknowledgments

## More Software development

Fully implement another small portion(s) of your project. As always, it is required that you thoroughly comment this component 
so that you can refer to it later to know what you were working on.

Be sure to identify which component you have fully implemented in your Scrum report.

---
# To Submit
In addition to creating the <code>test.plan.md</code> file, update <code>README.md</code> as necessary.

<sub>1. Adapted from [https://github.com/pearcej/pearcej.github.io/blob/master/csc493/r10-testing1.md](https://github.com/pearcej/pearcej.github.io/blob/master/csc493/r10-testing1.md)</sub>
