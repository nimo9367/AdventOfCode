count = 0

def checkpwd(line):
    words = []
    for part in line.split(" "):
        if part in words:
            return False
        else:
            words.append(part)
    return True

with open("2017-4.input", "r") as inpfile:
    for line in inpfile:
        if checkpwd(line):
            count += 1

print(checkpwd("aa bb cc dd aa")) 
print("Total valid pwds: " + str(count))
