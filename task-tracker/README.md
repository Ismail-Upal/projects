# Task Tracker CLI

A simple command-line interface (CLI) to manage tasks. Keep track of what you need to do, what you are doing, and what’s completed. Tasks are saved in a JSON file for persistence.

## Features

- Add, update, and delete tasks
- Mark tasks as **in-progress** or **done**
- List all tasks or filter by status
- Automatic JSON persistence (`tasks.json`)

## Task Structure

Each task has:

- `id` – Unique identifier
- `description` – Short task description
- `status` – `todo`, `in-progress`, `done`
- `createdAt` – Creation timestamp
- `updatedAt` – Last update timestamp

## Usage

Run the CLI:

```bash
# Add a task
task-cli add "Buy groceries"

# Update a task
task-cli update 1 "Buy groceries and cook dinner"

# Delete a task
task-cli delete 1

# Mark as in-progress
task-cli mark-in-progress 1

# Mark as done
task-cli mark-done 1

# List tasks
task-cli list
task-cli list done
task-cli list todo
task-cli list in-progress
