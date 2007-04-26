<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes" encoding="utf-8" method="html"/>
  <xsl:template match="/bugx">
    <xsl:text disable-output-escaping="yes">
      <![CDATA[<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strinct//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">]]>
    </xsl:text>
    <html>
      <head>
        <title>Error: <xsl:value-of select="url"/></title>
        <style type="text/css">
          /*<![CDATA[*/
          html, body
          {
            margin:0px;
            padding:0px;
            background:#F0F3F8;
            font:normal 10 px verdana;
          }
          fieldset
          {
          }
          legend
          {
            color:5B79CC;
          }
          /*]]>*/
        </style>
      </head>
      <body>
        Error on <xsl:value-of select="date"/>
        <fieldset>
          <legend>Complete error description:</legend>
          <xsl:if test="queryString/*">
            <fieldset>
              <legend>QueryString:</legend>
              <table>
                <xsl:for-each select="queryString/*">
                  <tr>
                    <th class="Variable" title="System.String">
                      <xsl:value-of select="name()"/>
                    </th>
                    <td>
                      <code>
                        <xsl:value-of select="."/>
                      </code>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
          <xsl:if test="form/*">
            <fieldset>
              <legend>Form:</legend>
              <table>
                <xsl:for-each select="form/*">
                  <tr>
                    <th class="Variable" title="System.String">
                      <xsl:value-of select="name()"/>
                    </th>
                    <td>
                    <code>
                      <xsl:value-of select="."/>
                    </code>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </fieldset>
          </xsl:if>
          <fieldset>
            <legend>Session:</legend>
            <table>
              <tr>
                <th>Code page:</th>
                <td>
                  <xsl:value-of select="sessionVariables/@page"/>
                </td>
              </tr>
              <tr>
                <th>Mode:</th>
                <td>
                  <xsl:value-of select="sessionVariables/@mode"/>
                </td>
              </tr>
              <tr>
                <th>Count:</th>
                <td>
                  <xsl:value-of select="count(sessionVariables/add)"/>
                </td>
              </tr>
            </table>
            <xsl:if test="sessionVariables/add">
              <table>
                <xsl:for-each select="sessionVariables/add">
                  <tr>
                    <th class="Variable">
                      <xsl:attribute name="title">
                        <xsl:value-of select="@type"/>
                      </xsl:attribute>
                      <xsl:value-of select="@name"/>
                    </th>
                    <td>
                      <xsl:choose>
                        <xsl:when test="@value">
                          <code>
                            <xsl:value-of select="@value"/>
                          </code>
                        </xsl:when>
                        <xsl:when test=".">
                          <code>(object)</code>
                        </xsl:when>
                        <xsl:otherwise>
                          <code class="Error">No data has been serialized for this object</code>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:if>
          </fieldset>
        </fieldset>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>