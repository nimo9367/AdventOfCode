import re
data = []
with open("2017-5.input", "r") as inpfile:
    for line in inpfile:
        data.append(line)

m = re.search('(?<=-)\w+', 'spam-egg')
print(m.group(0))