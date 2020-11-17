const jmxGenerator = ({
  url,
  method,
  loops,
  ramp_time,
  num_threads,
  stress_step,
  stress_qty,
  mass_data,
}) => {
  console.log(`Running Test with following properties:
  url = ${url},
  method = ${method},
  loops = ${loops},
  ramp_time = ${ramp_time},
  num_threads = ${num_threads},
  stress_step = ${stress_step},
  stress_qty = ${stress_qty},
  mass_data = ${mass_data},
  `);

  const testGroups = [];

  const urlMapper = new URL(url);

  for (let i = 0; i < stress_qty; i++) {
    const request = generateHTTPSamplerProxy({
      testname: `Test Request ${i + 1}`,
      domain: urlMapper.hostname,
      port: urlMapper.port,
      protocol: urlMapper.protocol.replace(":", ""),
      path: decodeURIComponent(urlMapper.pathname),
      method,
      mass_data,
    });
    const group = generateThreadGroup({
      testname: `Test Group ${i + 1}`,
      loops,
      num_threads: num_threads + stress_step * i,
      ramp_time,
      hashTree: request,
    });
    testGroups.push(group);
  }

  return generateTestPlan({
    testname: "Test Plan",
    hashTree: testGroups.join(""),
  });
};

const generateTestPlan = ({ testname, hashTree }) => `
<?xml version="1.0" encoding="UTF-8"?>
<jmeterTestPlan version="1.2" properties="5.0" jmeter="5.3">
  <hashTree>
    <TestPlan guiclass="TestPlanGui" testclass="TestPlan" testname="${testname}" enabled="true">
      <stringProp name="TestPlan.comments"></stringProp>
      <boolProp name="TestPlan.functional_mode">false</boolProp>
      <boolProp name="TestPlan.tearDown_on_shutdown">true</boolProp>
      <boolProp name="TestPlan.serialize_threadgroups">true</boolProp>
      <elementProp name="TestPlan.user_defined_variables" elementType="Arguments" guiclass="ArgumentsPanel" testclass="Arguments" testname="Vari�veis Definidas Pelo Usu�rio" enabled="true">
        <collectionProp name="Arguments.arguments"/>
      </elementProp>
      <stringProp name="TestPlan.user_define_classpath"></stringProp>
    </TestPlan>
    <hashTree>
    ${hashTree}
    </hashTree>
  </hashTree>
</jmeterTestPlan>
  `;

const generateThreadGroup = ({
  testname,
  loops,
  num_threads,
  ramp_time,
  hashTree,
}) => `
<ThreadGroup guiclass="ThreadGroupGui" testclass="ThreadGroup" testname="${testname}" enabled="true">
  <stringProp name="ThreadGroup.on_sample_error">continue</stringProp>
  <elementProp name="ThreadGroup.main_controller" elementType="LoopController" guiclass="LoopControlPanel" testclass="LoopController" testname="Controlador de Iteração" enabled="true">
    <boolProp name="LoopController.continue_forever">false</boolProp>
    <stringProp name="LoopController.loops">${loops}</stringProp>
  </elementProp>
  <stringProp name="ThreadGroup.num_threads">${num_threads}</stringProp>
  <stringProp name="ThreadGroup.ramp_time">${ramp_time}</stringProp>
  <boolProp name="ThreadGroup.scheduler">false</boolProp>
  <stringProp name="ThreadGroup.duration"></stringProp>
  <stringProp name="ThreadGroup.delay"></stringProp>
  <boolProp name="ThreadGroup.delayedStart">true</boolProp>
  <boolProp name="ThreadGroup.same_user_on_next_iteration">true</boolProp>
</ThreadGroup>
<hashTree>
  ${hashTree}
</hashTree>
  `;

const generateHTTPSamplerProxy = ({
  testname,
  domain,
  port,
  protocol,
  path,
  method,
  mass_data,
}) => `
<HTTPSamplerProxy guiclass="HttpTestSampleGui" testclass="HTTPSamplerProxy" testname="${testname}" enabled="true">
  <stringProp name="HTTPSampler.domain">${domain}</stringProp>
  <stringProp name="HTTPSampler.port">${port}</stringProp>
  <stringProp name="HTTPSampler.protocol">${protocol}</stringProp>
  <stringProp name="HTTPSampler.contentEncoding"></stringProp>
  <stringProp name="HTTPSampler.path">${path}</stringProp>
  <stringProp name="HTTPSampler.method">${method}</stringProp>
  <boolProp name="HTTPSampler.follow_redirects">true</boolProp>
  <boolProp name="HTTPSampler.auto_redirects">false</boolProp>
  <boolProp name="HTTPSampler.use_keepalive">true</boolProp>
  <boolProp name="HTTPSampler.DO_MULTIPART_POST">true</boolProp>
  <stringProp name="HTTPSampler.embedded_url_re"></stringProp>
  <stringProp name="HTTPSampler.connect_timeout"></stringProp>
  <stringProp name="HTTPSampler.response_timeout"></stringProp>
</HTTPSamplerProxy>

${
  mass_data
    ? `
<hashTree/>
<CSVDataSet guiclass="TestBeanGUI" testclass="CSVDataSet" testname="CSV Data Set Config" enabled="true">
  <stringProp name="delimiter">;</stringProp>
  <stringProp name="fileEncoding"></stringProp>
  <stringProp name="filename">${mass_data}</stringProp>
  <boolProp name="ignoreFirstLine">true</boolProp>
  <boolProp name="quotedData">false</boolProp>
  <boolProp name="recycle">true</boolProp>
  <stringProp name="shareMode">shareMode.all</stringProp>
  <boolProp name="stopThread">false</boolProp>
  <stringProp name="variableNames">id</stringProp>
</CSVDataSet>
`
    : ""
}

<hashTree>
  <ResponseAssertion guiclass="AssertionGui" testclass="ResponseAssertion" testname="${testname}" enabled="true">
    <collectionProp name="Asserion.test_strings">
      <stringProp name="status_code_ok">200</stringProp>
    </collectionProp>
    <stringProp name="Assertion.custom_message"></stringProp>
    <stringProp name="Assertion.test_field">Assertion.response_code</stringProp>
    <boolProp name="Assertion.assume_success">false</boolProp>
    <intProp name="Assertion.test_type">16</intProp>
  </ResponseAssertion>
  <hashTree/>
</hashTree>
  `;

module.exports = jmxGenerator;

/*

*/
