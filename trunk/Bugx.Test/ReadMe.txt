A mettre dans le web.config:

<configuration>
    <system.web>
      <httpModules>
        <add name="BugxWatermark" type="Bugx.Test.EnvironmentWatermark, Bugx.Test"/>
      </httpModules>
    </system.web>
    <!-- 
        Section pour IIS 7
    -->
    <system.webServer>
      <modules>
        <add name="BugxWatermark" type="Bugx.Test.EnvironmentWatermark, Bugx.Test"/>
      </modules>
    </system.webServer>
</configuration>
