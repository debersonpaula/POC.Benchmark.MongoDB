var path = require("path");

module.exports = {
  runOptions: {
    // url: "http://localhost:4000/api/collection/${id}",
    url: "http://localhost:4000/api/document/${id}",
    method: "GET",
    loops: 1,
    ramp_time: 5,
    num_threads: 1000,
    stress_step: 200,
    stress_qty: 6,

    // ramp_time: 1,
    // num_threads: 2000,
    // stress_step: 0,
    // stress_qty: 1,
    mass_data: path.resolve(__dirname, "mass.csv"),
  },
  // jvmArgs: 'set JVM_ARGS="-Xms512m -Xmx4096m -Dpropname=value"',
  jvmArgs: 'set JVM_ARGS="-Xms512m -Xmx4096m -Dpropname=value"',
  jmeterPath: "C:\\apps\\apache-jmeter-5.3\\bin\\",
};
