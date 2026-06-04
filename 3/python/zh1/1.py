d = {"Adam" : 11, "Jerry" : 4, "Michael" : 45, "Ben" : 10}
gt10 = [name for name, age in d.items() if age > 10]
print(gt10)