import os
def compare_files(file1_path, file2_path):
    with open(file1_path, 'r') as file1, open(file2_path, 'r') as file2:
        content1 = file1.read()
        content2 = file2.read()

        if content1 == content2:
            print("Files are identical.")
        else:
            print("Files are different.")


files = os.listdir()
outputs = list(filter(lambda file: file[:4] == 'outp', files)
expected = list(filter(lambda file: file[-7:] == 'put.txt', files)
for i in range(len(outputs)):
	compare_files(outputs[i], expected[i])