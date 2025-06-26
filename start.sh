#!/bin/sh

if ! ollama list | grep -q llama3; then
  echo "Model llama3 not found. Starting server in background to pull it..."
  ollama serve &
  sleep 5
  ollama pull llama3
fi

echo "Starting Ollama server..."
exec ollama serve
