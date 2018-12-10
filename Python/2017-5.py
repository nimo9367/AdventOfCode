data = []
with open("2017-5.input", "r") as inpfile:
    for line in inpfile:
        data.append(int(line))
        
idx = 0
cnt = 0
prev = 0
while idx < len(data):
    cnt += 1
    jump = data[idx]
    if jump == 0:
        data[idx] = 1
    else:
        if jump >= 3:
            data[idx] = data[idx] - 1
        else:
            data[idx] += 1
        idx += jump

print(cnt)