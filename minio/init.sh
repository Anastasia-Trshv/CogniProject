#!/bin/sh
set -e

minio server /data --console-address ":9090" &

echo "Waiting for MinIO to start..."
while ! curl -s "http://minio:9000/minio/health/live" >/dev/null; do
  sleep 1
done
echo "MinIO is up!"

echo "Setting up MinIO with alias 'local' at $MINIO_HOST"
mc alias set local "$MINIO_HOST" "$MINIO_ROOT_USER" "$MINIO_ROOT_PASSWORD"
mc mb --ignore-existing "local/$BUCKET_NAME"
mc anonymous set download "local/$BUCKET_NAME"
echo "MinIO bucket '$BUCKET_NAME' configured successfully!"

wait
