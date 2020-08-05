def BracketMatcher(str):

  #return value
  ret = 1

  #amount of brackets left open
  open_brackets = 0

  for i in str:
    if(i == "("):
      open_brackets += 1
    elif(i == ")"):
      open_brackets -= 1

    if(open_brackets < 0):
      ret = 0
      break
  if(open_brackets != 0):
    ret = 0
    
  return ret

# keep this function call here 
print(BracketMatcher(input()))