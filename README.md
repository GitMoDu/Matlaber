Matlaber
========

Matlaber is a translation software to simulate PLC control programs in the Matlab/Simulink environment, taking advantage of the IEC 61131 standard. 

PLC code in standard IEC-61131-3 IL is translated into a Matlab m-file(s) which can then be inserted in a Simulink "function" box, with the adequate inputs and outputs. With this, the "function" box behaves exactly like the PLC program in IL should.
This provides a simulated assessment of the industrial process modelled by the PLC program, making easier its test and validation. Although designed for teaching, both method and tool can also be applied in industrial projects.

Motivation
========

Matlab/Simulink is a common testing environment for processes, but to test an industrial that is controled by a PLC, the PLC and it's program is missing. The presented work is a solution to that, enabling the simulation of PLC programs inside the Matlab/Simulink environment.

Context
========
This work is based on the Master's thesis of Andre Caldeira Pereira, which is available in full (but only in Portuguese) at FCT-UNL repository: http://run.unl.pt/bitstream/10362/6871/1/Pereira_2011.pdf

The work in the thesis also provides another software tool, one that converts proprietary PLC code into standard IEC-61131-3 IL, and is available at https://github.com/GitMoDu/Unif-IL .

Dependencies
========
Matlaber runs on pure .NET 3.5 , mainly for the interface and string handling functions. It's porting to other languages should be hassle free.
It also uses one special m-file that enables the GOTO functionality, found in IL code, to work in Matlab.
It's credit goes to Husam Aldahiyat who published the file and graciously allowed it to be used in the this work. 
This file is included in the Matlaber project, was published under a BSD license and can be found here: http://www.mathworks.com/matlabcentral/fileexchange/26949-matlab-goto-statement

