
def binarySearch(arr, x):
    low = 0
    high = len(arr) - 1
    mid = 0
    while low <= high:
        mid = (high + low) // 2
        if arr[mid] < x:
            low = mid + 1
        elif arr[mid] > x:
            high = mid - 1
        else:
            return mid
    return -1


def lastTwo(string):
    return string[-2:]
pairs = {}
def testing():
    with open('noi/ruleofthree.txt', 'r') as f:
        n = int(f.readline())
        aintegers = list(map(int, map(lastTwo, f.readline().split())))
        bintegers = list(map(int, map(lastTwo, f.readline().split())))
        cintegers = list(map(int, map(lastTwo, f.readline().split())))
        aintegers.sort()
        bintegers.sort()
        cintegers.sort()
        count = 0
        for a in aintegers:
            for b in bintegers:
                tofind = 100 - a - b
                if tofind == 100:
                    tofind = 0
                while tofind < 0:
                    tofind += 100
                start = binarySearch(cintegers, tofind)
                if start == -1:
                    continue
                
                low = start
                high = start
                for i in range(start, len(cintegers)):
                    if cintegers[i] != tofind:
                        break
                    high = i
                for i in range(start, -1, -1):
                    if cintegers[i] != tofind:
                        break
                    low = i
                count += high - low + 1
                key = (a, b, tofind)
                if key in pairs.keys():
                    pairs[key] += high - low + 1
                else:
                    pairs[key] = high - low + 1
        print(count)
    with open('noi/output.txt', 'w') as f:
        keys = list(pairs.items())
        keys.sort(key = lambda x : (x[0][0],x[0][1],x[0][2]))
        for key in keys:
            f.write(f'{key[0]} {key[1]} \n')

def main():
    n = int(input())
    aintegers = list(map(int, map(lastTwo, input().split())))
    bintegers = list(map(int, map(lastTwo, input().split())))
    cintegers = list(map(int, map(lastTwo, input().split())))
    aintegers.sort()
    bintegers.sort()
    cintegers.sort()
    count = 0
    for a in aintegers:
        for b in bintegers:
            tofind = 100 - a - b
            if tofind == 100:
                tofind = 0
            while tofind < 0:
                tofind += 100
            start = binarySearch(cintegers, tofind)
            if start == -1:
                continue
            
            low = start
            high = start
            for i in range(start, len(cintegers)):
                if cintegers[i] != tofind:
                    break
                high = i
            for i in range(start, -1, -1):
                if cintegers[i] != tofind:
                    break
                low = i
            count += high - low + 1
    print(count)


testing()
