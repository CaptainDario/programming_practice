

% Richtungen und Zeitraum.
dir(-1; 1).
time(1..M) :- minMax(M).

#minimize { T@3 : allDone(T) }.
#minimize { T@2, E : serve(E, T) }.
#minimize { T@1, E, D : move(E, D, T)}.




% Alle moeglichen Aktionen eines Fahrstuhls zu jedem Zeitpunkt.
%optim.: nach oben/unten fahren nur mgl. wenn dort auch ein call/deliver ist
{ 
  move(E, 1, T)  : dir(D), location(E, F, T), call(F2),       F < F2, not minMax(T); 
  move(E, 1, T)  : dir(D), location(E, F, T), deliver(E, F2), F < F2, not minMax(T);
  move(E, -1, T) : dir(D), location(E, F, T), call(F2),       F > F2, not minMax(T); 
  move(E, -1, T) : dir(D), location(E, F, T), deliver(E, F2), F > F2, not minMax(T); 
  serve(E, T) 
} 1 :- elevator(E), time(T).

%%% nicht nach oben, wenn dort alle calls/delivers schon abgearbeitet sind
:- move(E, -1, T), location(E, F, T), not call(F1), cDone(F1, _), F1 < F. 


% Hilfspraedikat mit "passender" Location zu einem Serve.
serves(E, F, T) :- location(E, F, T), serve(E, T), deliver(E,F).
serves(E, F, T) :- location(E, F, T), serve(E, T), call(F), not deliver(_, F).

% Die init-Position eines Fahrstuhls setzen.
location(E, F, 1) :- init(E, F).

% Update die Location eines Fahrstuhls, wenn er sich bewegt.
location(E, F, T+1) :- location(E, F-D, T), move(E, D, T), floor(F).

% Update die Location eines Fahrstuhls, wenn er ein Serve ausfuehrt.
location(E, F, T+1) :- serves(E, F, T), not horizon(T), not horizon(T+1).

% ueberpruefe die "Ausfuehrbarkeit" von Aktionen.
:- move(E, _, T), not location(E, _, T+1).
:- serve(E, T),   not location(E, F, T) : deliver(E, F);
                  not location(E, F, T) : call(F), not deliver(_, F).

% Wenn es ein Deliver an einer Location gibt, muss dort auch (genau) ein Serve sein.
:- deliver(E, F), #count{ T : serves(E, F, T) } != 1.

% Wenn es einen Call an einer Location gibt, muss dort auch (genau) ein Serve sein.
:- call(F), not deliver(_, F), #count{ E, T : serves(E, F, T) } != 1.


%%% SHITTY RULE OPTIMIZATION %%%

%%% NR. 1 %%% 
% sind alle call/deliveries abgearbeitet
%--> Zeitpunkt des als letzten abgearbeiteten deliver/calls minimieren 

%ein call ist abgearbeitet
cDone(F, T) :- call(F), location(E, F, T), serve(E, T).
allCDone(B, T) :- call(F), cDone(F, T), B = 1.

%ein delivery ist abgearbeitet
dDone(F, T) :- deliver(E, F), location(E, F, T), serve(E, T).
allDDone(B, T) :- deliver(E, F), dDone(F, T), B = 1.

%alle calls und deliveries sind abgearbeitet
allDone(T3) :- T3 = #max { T1 : allCDone(_, T1); T2 : allDDone(_, T2) }.


%allDone muss gelten
:- not allDone(_).


%%%%%%%%%%%%%

%%% Nr. 2 %%%
% ein Fahrstuhl sollte nicht mehr als ein Mal die Richtung wechseln
:- move(E,  1, T1), move(E, -1, T2), move(E,  1, T3), T1 < T2, T2 < T3.
:- move(E, -1, T1), move(E,  1, T2), move(E, -1, T3), T1 < T2, T2 < T3.

%%%%%%%%%%%%%

%%% Nr. 3 %%%
% ein Fahrstuhl kann zu jedem Zeitpunkt NUR Server ODER Move
:- serve(E, T), move(E, _, T), elevator(E), time(T).

%%%%%%%%%%%%%


% reduce Horizon
%Anzahl von Feldern auf denen entweder ein Call oder ein deilvery ist
amountBoth(AB) :- AB = #count { Y : call(Y) ; Y : deliver(_, Y) }.
%Gesamtanzahl von floor
amountFloor(AF) :- AF = #count {F : floor(F)}.
%possible Time
minMax(T) :- amountBoth(AB), amountFloor(AF), T = AB + AF + AF / 2.

#show move/3.
#show serve/2.
%#show cDone/2.
%#show allCDone/2.
%#show allDDone/2.
%#show allDone/1.
%#show amountBoth/1.
%#show amountFloor/1.
%#show minMax/1.
%#show horizon/1.
