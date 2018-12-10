import re
import json
from operator import itemgetter

class GuardBlock(object):
    def __init__(self, **kwargs):
        self.__dict__.update(kwargs)

data = []
genregex = "\[(.*)\s([0-9]*):([0-9]*)\]\s(.*)"
guardregex = "Guard #([0-9]*) begins shift"
with open("2018-4.input", "r") as inpfile:
    for line in inpfile:
        data.append(line)


guards = {}
data.sort()
guardid = None
sleepminute = 0
awakeminute = 0
sleeping = False
for event in data:
        date = None
        match = re.search(genregex, event)
        if match:
                day = match.group(1)
                hour = int(match.group(2))
                minute = int(match.group(3))
                action = match.group(4)
                if hour is not 0:
                        minute = 0
                if action.startswith("Guard"):
                        guardmatch = re.search(guardregex, action)
                        guardid = guardmatch.group(1)
                        key = guardmatch.group(1)
                        guardminute = minute
                        if key not in guards:
                                guards[key] = []
                        awakeminute = 0
                        sleepminute = 0
                elif action.startswith("falls asleep"):
                        sleepminute = minute
                elif action.startswith("wakes up"):
                        awakeminute = minute
                        guards[key].append({ "day": day, "minute": sleepminute, "time": awakeminute - sleepminute, "sleepminute": sleepminute, "awakeminute": awakeminute })

result = []
for key in guards:
        guards[key] = sorted(guards[key], key=itemgetter('time'), reverse=True)
        res = list(map(lambda x: x["time"], guards[key]))
        minutes = {}
        for minute in range(60):
                for ev in guards[key] :
                        if ev["sleepminute"] <= minute and ev["awakeminute"] > minute:
                                if minute in minutes:
                                        minutes[minute] = minutes[minute] + 1
                                else:
                                        minutes[minute] = 1
        sortedmins = sorted(minutes.items(), key=lambda kv: kv[1], reverse=True)
        if len(res) > 0:
                result.append({"guard": key, "sum": sum(res), "minute": sortedmins[0][0], "hotminute": sortedmins[0][1] })
        else:
                result.append({"guard": key, "sum": 0, "minute": 0, "hotminute": 0})
result.sort(key=itemgetter('hotminute'), reverse=True)
print(json.dumps(result))