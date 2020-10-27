% INSERT NAMES AND EMAIL ADDRESSES OF YOUR GROUP MEMBERS:
% 
%



% Das Spielfeld
wert(1..X) :- size(X).
cell(1..N,1..N) :- size(N).
dir(0, 1).
dir(1, 0).
dir(-1, 0).
dir(0, -1).
reach(X,Y,DX,DY,X+DX,Y+DY) :- planet(X,Y), dir(DX,DY), cell(X+DX,Y+DY).
reach(X,Y,DX,DY,X1+DX,Y1+DY) :- reach(X,Y,DX,DY,X1,Y1), cell(X1+DX,Y1+DY).

% Planet != Wolke / Stern
:- planet(X, Y), cloud(X, Y).
:- planet(X, Y), star(X, Y).

% Nur EIN Stern pro Zeile/Spalte
{star(X, Y) : wert(Y)} = 1 :- wert(X).
{star(X, Y) : wert(X)} = 1 :- wert(Y).

% Nur EINE Wolke pro Zeile/Spalte
{cloud(X, Y) : wert(Y)} = 1 :-  wert(X).
{cloud(X, Y) : wert(X)} = 1 :-  wert(Y).

% Nur Stern ODER Wolke auf einem Feld
:- star(X1, Y), cloud(X2, Y), X1 = X2.
:- star(X, Y1), cloud(X, Y2), Y1 = Y2.

% Wenn Seite hell, platziere dort Stern
:- bright(X1, Y , -1,  _), star(X2, Y ), X1 <= X2.
:- bright(X , Y1,  _, -1), star(X , Y2), Y1 <= Y2.
:- bright(X1, Y ,  1,  _), star(X2, Y ), X1 >= X2.
:- bright(X , Y1,  _,  1), star(X , Y2), Y1 >= Y2.

% Wenn Seite hell, keine Wolke zwischen Planet & Stern
:- bright(X1, Y , -1, DY), star(X2, Y ), cloud(X3, Y ), X3 >= X2, X3 <= X1.
:- bright(X , Y1, DX, -1), star(X , Y2), cloud(X , Y3), Y3 >= Y2, Y3 <= Y1.
:- bright(X1, Y , 1 , DY), star(X2, Y ), cloud(X3, Y ), X3 <= X2, X3 >= X1.
:- bright(X , Y1, DX,  1), star(X , Y2), cloud(X , Y3), Y3 <= Y2, Y3 >= Y1.

% Wenn Seite dunkel, kein Stern ohne Wolke
:- planet(X1, Y1), reach(X1, Y1,  1, 0, X2, Y1), reach(X1, Y1, -1, 0,  X3, Y1), not bright(X1, Y1,  1,  0), star(X2, Y1), cloud(X3, Y1).
:- planet(X1, Y1), reach(X1, Y1, -1, 0, X2, Y1), reach(X1, Y1,  1, 0,  X3, Y1), not bright(X1, Y1, -1,  0), star(X2, Y1), cloud(X3, Y1).
:- planet(X1, Y1), reach(X1, Y1, 0,  1, X1, Y2), reach(X1, Y1,  0, -1, X1, Y3), not bright(X1, Y1,  0,  1), star(X1, Y2), cloud(X1, Y3).
:- planet(X1, Y1), reach(X1, Y1, 0, -1, X1, Y2), reach(X1, Y1,  0,  1, X1, Y3), not bright(X1, Y1,  0, -1), star(X1, Y2), cloud(X1, Y3).

% Wenn Seite dunkel, dann Wolke zwischen Stern & Planet
:- planet(X1, Y1), reach(X1, Y1,  1, 0, X2, Y1), reach(X1, Y1,  1, 0, X3, Y1), not bright(X1, Y1, 1,  _), star(X2, Y1), cloud(X3, Y1), |X1 - X2| < |X1 - X3|.
:- planet(X1, Y1), reach(X1, Y1, -1, 0, X2, Y1), reach(X1, Y1, -1, 0, X3, Y1), not bright(X1, Y1, -1,  _), star(X2, Y1), cloud(X3, Y1), |X1 - X2| < |X1 - X3|.
:- planet(X1, Y1), reach(X1, Y1, 0,  -1, X1, Y2), reach(X1, Y1,  0, -1, X1, Y3), not bright(X1, Y1,  _,  -1), star(X1, Y2), cloud(X1, Y3), |Y1 - Y2| < |Y1 - Y3|.
:- planet(X1, Y1), reach(X1, Y1, 0,   1, X1, Y2), reach(X1, Y1,  0,  1, X1, Y3), not bright(X1, Y1,  _,   1), star(X1, Y2), cloud(X1, Y3), |Y1 - Y2| < |Y1 - Y3|.
 
#show star/2.
#show cloud/2.

