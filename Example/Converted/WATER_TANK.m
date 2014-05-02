function [OUTPUT_VECTOR]=WATER_TANK(CLOCK_IN,EXTERNAL_COUNTER_RESET,MAXIMUM_LEVEL_SWITCH,MINIMUM_LEVEL_SWITCH)
%CONTROLS THE WATER LEVEL OF A TANK

%Converted by IEC-Matlab converter by Andre' Pereira
%02/05/2014 14:01:07 GMT Standard Time


global ALARM_COUNTER BUZZ_SIGNAL_ALARM COUNTER1 COUNTER1_FBID LIGHT_SIGNAL_ALARM TIMER1 TIMER1_FBID VALVE


%--------------------------Variables' comments---------------------------------
%global COUNTER1_FBID's comment: FB identifier
%global TIMER1_FBID's comment: FB identifier
%------------------------------------------------------------------------------


VALVE = 0;
COUNTER1(2) = 0; % COUNTER1.CU
if (MAXIMUM_LEVEL_SWITCH==0)
    goto('NOTMAXIMUM');
    return % Part of the goto functionality in Matlab, ignore
end
VALVE = 1;
COUNTER1(2) = 1; % COUNTER1.CU
goto('ALLDONE');
return % Part of the goto functionality in Matlab, ignore
% NOTMAXIMUM LABEL'S COMMENT: 
% LABEL NOTMAXIMUM
if (MINIMUM_LEVEL_SWITCH==0)
    goto('NOTMINIMUM');
    return % Part of the goto functionality in Matlab, ignore
end
VALVE = 0;
COUNTER1(2) = 0; % COUNTER1.CU
goto('ALLDONE');
return % Part of the goto functionality in Matlab, ignore
% NOTMINIMUM LABEL'S COMMENT: 
% LABEL NOTMINIMUM
VALVE = 0;
COUNTER1(2) = 0; % COUNTER1.CU
% LABEL ALLDONE
COUNTER1(5)=EXTERNAL_COUNTER_RESET;COUNTER1(4)=8;COUNTER1=COUNTER_UP(COUNTER1);ALARM_COUNTER=COUNTER1(3);
TIMER1(2)=COUNTER1(6);TIMER1(4)=3;TIMER1=TIMER_ON(CLOCK_IN,TIMER1); % COUNTER1.Q
BUZZ_SIGNAL_ALARM = TIMER1(6); % COUNTER1.QTIMER1.Q
LIGHT_SIGNAL_ALARM = COUNTER1(6); % COUNTER1.Q

%Output generation
OUTPUT_VECTOR=[ALARM_COUNTER*1,BUZZ_SIGNAL_ALARM*1,LIGHT_SIGNAL_ALARM*1,VALVE*1];
