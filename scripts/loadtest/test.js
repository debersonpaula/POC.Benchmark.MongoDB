const path = require("path");
const fs = require("fs");
const jmxGenerator = require("./tools/jmxGenerator");
const config = require("./config");
const execSync = require("child_process").execSync;

const jmeterPath = config.jmeterPath;
const jmeterOutput = path.resolve(__dirname, "tmp");
const jmeterOutputHtml = path.resolve(__dirname, "tmp/html");
const jmeterTestFile = path.resolve(jmeterOutput, "test-script.jmx");

process.on("unhandledRejection", (err) => {
  throw err;
});

createPath(jmeterOutput);
createPath(jmeterOutputHtml);
const testScript = jmxGenerator(config.runOptions);

fs.writeFileSync(jmeterTestFile, testScript);

const command =
  jmeterPath +
  `jmeter -n -t ${jmeterTestFile} -l ${jmeterOutput}/load-test-results.jtl -e -o ${jmeterOutputHtml}`;
console.log("Executing command", command);

execSync(config.jvmArgs, {
  stdio: "inherit",
});
execSync(command, { stdio: "inherit" });

function createPath(pathname) {
  if (!fs.existsSync(pathname)) {
    fs.mkdirSync(pathname);
  }
}
