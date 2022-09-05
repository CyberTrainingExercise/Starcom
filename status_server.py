"""
Handles REST requests from UI and Satellites nodes.

Takes get requests from UI for status info.

Takes post requests from Sats for status updates.
"""

from flask import Flask, jsonify, request
import datetime

app = Flask(__name__)

start_status = datetime.datetime.now()

# Status table
status = {
   "status0": start_status,
   "status1": start_status,
   "status2": start_status
}


@app.route('/status')
def get_status():
    """
    Returns status of all satellites
    """
    goal = datetime.datetime.now() - datetime.timedelta(seconds=5)
    data = {
        "status0": "offline",
        "status1": "offline",
        "status2": "offline"
    }
    # compare status with goal time
    if (status["status0"] > goal):
        data["status0"] = "ok"
    if (status["status1"] > goal):
        data["status1"] = "ok"
    if (status["status2"] > goal):
        data["status2"] = "ok"
    return jsonify(data)


@app.route('/status', methods=['POST'])
def update_status():
    """
    Updates status of a sat in the table

    Request format: "statusX:ok/offline"
    Example (sat0 is online): "status0:ok"
    """
    input = request.get_json()
    now = datetime.datetime.now()
    # update status with current time
    if ("status0" in input):
        status["status0"] = now
    if ("status1" in input):
        status["status1"] = now
    if ("status2" in input):
        status["status2"] = now
    return '', 204

if __name__ == '__main__':
    app.run(debug=True)