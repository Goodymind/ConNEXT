extends Node

signal file_selected_signal(path)

@onready var file_dialog = get_node('NativeFileDialog')

func select_file():
        file_dialog.show()

func file_selected(path):
        file_selected_signal.emit(path);
        print(path)
        